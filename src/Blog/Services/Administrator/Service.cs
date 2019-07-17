using AutoMapper;
using Blog.Domain;
using Blog.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.Administrator
{
   public class Service
   {
      public Service(BlogContext context, IMapper mapper, IImageContext imageContext, DraftValidator validator)
      {
         _context = context;
         _mapper = mapper;
         _imageContext = imageContext;
         _validator = validator;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;
      private readonly IImageContext _imageContext;
      private readonly DraftValidator _validator;

      public DraftEntry Create() =>
          new DraftEntry { PublishDate = DateTime.Now };

      public IEnumerable<DraftRow> GetDrafts() =>
          (from info in _context.Infos
           join post in _context.Posts on info.Id equals post.Id into posts
           from post in posts.DefaultIfEmpty()
           select _mapper.Map<DraftRow>(Tuple.Create(info, post == null ? -1 : post.Id))
           ).ToArray();

      public DraftEntry Get(int id) =>
          (from draft in _context.Drafts.Include(x => x.Info)
           join post in _context.Posts on draft.Id equals post.Id into posts
           from post in posts.DefaultIfEmpty()
           where draft.Id == id
           select _mapper.Map<DraftEntry>(Tuple.Create(draft, post == null ? -1 : post.Id))
           ).Single();

      public Home.PostViewModel GetView(int id)
      {
         var draft = _context
            .Drafts
            .Include(x => x.Info)
            .SingleOrDefault(x => x.Id == id);
         if (draft == null) return null;
         return _mapper.Map<Home.PostViewModel>(draft.Publish());
      }

      public SaveResult Save(DraftEntry viewModel)
      {
         var draft = _mapper.Map<Draft>(viewModel);

         var result = _validator.Validate(draft);
         if (result.Any())
            return SaveResult.Failure(result);

         var url = string.Empty;
         var images = draft.RenderImages();

         var oldDraft = _context.Drafts.Include(x => x.Info).SingleOrDefault(x => x.Id == draft.Id);
         var oldPostDirectory = oldDraft?.Info?.Slugify();
         if (oldDraft == null)
         {
            _context.Drafts.Add(draft);
         }
         else
         {
            _context.Entry(oldDraft).CurrentValues.SetValues(draft);
            _context.Entry(oldDraft.Info).CurrentValues.SetValues(draft.Info);
         }

         if (viewModel.Publish)
         {
            var post = draft.Publish();
            _context.AddOrUpdate(post);
            url = post.Url;
         }
         else if (_context.Posts.Any(draft.Id))
         {
            _context.Posts.Delete(draft.Id);
            _context.PostContents.Delete(draft.Id);
         }

         _context.SaveChanges();

         _imageContext.SaveChanges(oldPostDirectory, draft.Info.Slugify(), images);
         return SaveResult.Success(url);
      }

      public void Delete(int id)
      {
         var info = _context.Infos.Find(id);
         _context.Drafts.Delete(id);
         _context.SaveChanges();
         _imageContext.Delete(info.Slugify());
      }
   }
}
