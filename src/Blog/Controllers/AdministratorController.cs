using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Services.Administrator;
using Blog.Services.Home;
using Blog.Storage;
using Blog.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Blog.Controllers
{
   [Authorize]
   public class AdministratorController : Controller
   {
      public AdministratorController(
         BlogContext blogContext,
         ImageContext imageContext,
         IDateProvider dateProvider,
         IHtmlProcessor htmlProcessor,
         IStorageState storageState,
         IMapper mapper)
      {
         _context = blogContext;
         _imageContext = imageContext;
         _dateProvider = dateProvider;
         _htmlProcessor = htmlProcessor;
         _storageState = storageState;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly ImageContext _imageContext;
      private readonly IDateProvider _dateProvider;
      private readonly IHtmlProcessor _htmlProcessor;
      private readonly IStorageState _storageState;
      private readonly IMapper _mapper;

      public ViewResult Index()
      {
         var drafts = (from info in _context.Drafts
                       join post in _context.Posts on info.Id equals post.Id into posts
                       from post in posts.DefaultIfEmpty()
                       select _mapper.Map<DraftRow>(Tuple.Create(info, post == null ? -1 : post.Id))
          ).ToArray();

         return View(drafts);
      }

      public ViewResult Post() => View(new DraftEntry());

      public IActionResult ViewPost(int id)
      {
         var re = (from draft in _context.Drafts
                   join post in _context.Posts on draft.Id equals post.Id into posts
                   from post in posts.DefaultIfEmpty()
                   where draft.Id == id
                   select _mapper.Map<DraftEntry>(Tuple.Create(draft, post == null ? -1 : post.Id))
           ).Single();

         if (re == null)
            return NotFound();
         return View(nameof(Post), re);
      }

      [ValidateAntiForgeryToken]
      public IActionResult SavePost(DraftEntry draftEntry)
      {
         if (!ModelState.IsValid)
            return View(nameof(Post), draftEntry);

         var command = new DraftUpdateCommand
         {
            Content = draftEntry.Content,
            EnglishUrl = draftEntry.EnglishUrl,
            Language = draftEntry.Language,
            Summary = draftEntry.Summary,
            Tags = draftEntry.Tags,
            Title = draftEntry.Title,
            Id = draftEntry.Id
         };

         Draft draft;
         if (command.Id == 0)
         {
            draft = new Draft();
            _context.Drafts.Add(draft);
         }
         else
         {
            draft = _context.GetDraft(command.Id);
         }

         draft.Update(command, _storageState);

         _context.SaveChanges();
         _imageContext.SaveChanges();

         if (draftEntry.Publish)
         {
            draft.Publish(_dateProvider, _htmlProcessor);
            _context.SaveChanges();
            var lang = draft.Language == Language.English ? "en" : "fa";
            return RedirectToAction("Post", "Home", new { language = lang, urlTitle = draft.Post.Url });
         }

         if (!draftEntry.Publish && draft.Post != null)
         {
            draft.Unpublish();
            _context.SaveChanges();
         }

         return RedirectToAction(nameof(Index));
      }

      [ValidateAntiForgeryToken]
      public IActionResult DeletePost(int id)
      {
         var draft = _context.Drafts.Find(id);
         _context.Drafts.Remove(draft);

         var post = _context.Posts.SingleOrDefault(x => x.Id == id);
         if (post != null)
         {
            _context.Posts.Remove(post);
         }

         _context.SaveChanges();
         _imageContext.SaveChanges();

         return RedirectToAction(nameof(Index));
      }

      public IActionResult Preview(int id)
      {
         var draft = _context
            .Drafts
            .SingleOrDefault(x => x.Id == id);
         if (draft == null) return null;

         draft.Publish(_dateProvider, _htmlProcessor);

         var post = _mapper.Map<PostViewModel>(draft.Post);

         if (post == null)
            return NotFound();

         ViewData["language"] = post.Language;
         return View("Views/Home/Post.cshtml", post);
      }

      [IgnoreMigration]
      public IActionResult Developer() =>
         View();
   }
}