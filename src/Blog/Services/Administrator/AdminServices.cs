using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Storage;
using Blog.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.Administrator
{
   public interface IAdminServices : IDisposable
   {
      DraftEntry Create();
      IEnumerable<DraftRow> GetDrafts();
      DraftEntry Get(int id);
      Home.PostViewModel GetView(int id);
      SaveResult Save(DraftEntry viewModel);
      void Delete(int id);
   }

   public class AdminServices : IAdminServices
   {
      public AdminServices(BlogContext context,
         IMapper mapper,
         IHtmlProcessor processor,
         ImageContext imageContext,
         IDateProvider dateProvider)
      {
         _context = context;
         _mapper = mapper;
         _processor = processor;
         _imageContext = imageContext;
         _dateProvider = dateProvider;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;
      private readonly IHtmlProcessor _processor;
      private readonly ImageContext _imageContext;
      private readonly IDateProvider _dateProvider;
      private readonly IStorageState _storageState;

      public DraftEntry Create() =>
          new DraftEntry();

      public IEnumerable<DraftRow> GetDrafts() =>
          (from info in _context.Drafts
           join post in _context.Posts on info.Id equals post.Id into posts
           from post in posts.DefaultIfEmpty()
           select _mapper.Map<DraftRow>(Tuple.Create(info, post == null ? -1 : post.Id))
           ).ToArray();

      public DraftEntry Get(int id) =>
          (from draft in _context.Drafts
           join post in _context.Posts on draft.Id equals post.Id into posts
           from post in posts.DefaultIfEmpty()
           where draft.Id == id
           select _mapper.Map<DraftEntry>(Tuple.Create(draft, post == null ? -1 : post.Id))
           ).Single();

      public Home.PostViewModel GetView(int id)
      {
         var draft = _context
            .Drafts
            .SingleOrDefault(x => x.Id == id);
         if (draft == null) return null;

         return _mapper.Map<Home.PostViewModel>(draft.Publish(_dateProvider, _processor, _storageState));
      }

      public SaveResult Save(DraftEntry viewModel)
      {
         return null;
         //_saveCommand.Run(viewModel);
      }

      public void Delete(int id)
      {
         var draft = _context.Drafts.Find(id);
         _context.Drafts.Remove(draft);

         var post = _context.Posts.SingleOrDefault(x => x.Id == id);
         if (post != null)
         {
            _context.Posts.Remove(post);
         }

         _context.SaveChanges();
         _imageContext.Delete(draft.Slugify());
      }

      public void Dispose()
      {
         _context.Dispose();
         //_saveCommand.Dispose();
      }
   }
}
