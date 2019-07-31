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
      public Service(BlogContext context,
         IMapper mapper,
         IImageContext imageContext,
         ICodeFormatter codeFormatter,
         IImageProcessor imageProcessor,
         DraftSaveCommand saveCommand)
      {
         _context = context;
         _mapper = mapper;
         _imageContext = imageContext;
         _codeFormatter = codeFormatter;
         _imageProcessor = imageProcessor;
         _saveCommand = saveCommand;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;
      private readonly IImageContext _imageContext;
      private readonly ICodeFormatter _codeFormatter;
      private readonly IImageProcessor _imageProcessor;
      private readonly DraftSaveCommand _saveCommand;

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
         return _mapper.Map<Home.PostViewModel>(draft.Publish(_codeFormatter, _imageProcessor));
      }

      public SaveResult Save(DraftEntry viewModel) =>
         _saveCommand.Run(viewModel);

      public void Delete(int id)
      {
         var info = _context.Infos.Find(id);
         _context.Drafts.Delete(id);
         _context.SaveChanges();
         _imageContext.Delete(info.Slugify());
      }
   }
}
