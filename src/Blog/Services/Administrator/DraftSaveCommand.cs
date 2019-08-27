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
   public class DraftSaveCommand : IDisposable
   {
      public DraftSaveCommand(BlogContext context,
         IMapper mapper,
         DraftValidator validator,
         IImageContext imageContext,
         IHtmlProcessor processor,
         IDateProvider dateProvider)
      {
         _context = context;
         _mapper = mapper;
         _validator = validator;
         _imageContext = imageContext;
         _processor = processor;
         _dateProvider = dateProvider;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;
      private readonly DraftValidator _validator;
      private readonly IImageContext _imageContext;
      private readonly IHtmlProcessor _processor;
      private readonly IDateProvider _dateProvider;
      private Draft _draft;
      private IEnumerable<Image> _images;
      private string _oldPostDirectory;

      public SaveResult Run(DraftEntry viewModel)
      {
         if (viewModel.Id == 0)
         {
            _draft = new Draft();
            _context.Drafts.Add(_draft);
         }
         else
         {
            _draft = _context.Drafts.Find(viewModel.Id);
            _oldPostDirectory = _draft?.Slugify();
         }

         _draft.Content = viewModel.Content;
         _draft.EnglishUrl = viewModel.EnglishUrl;
         _draft.Language = viewModel.Language;
         _draft.Summary = viewModel.Summary;
         _draft.Tags = viewModel.Tags;
         _draft.Title = viewModel.Title;

         var validationResult = _validator.Validate(_draft);
         if (validationResult.Any())
            return SaveResult.Failure(_draft.Id, validationResult);

         _images = _draft.RenderImages();

         if (viewModel.Publish)
         {
            return PublishPost();
         }

         if (_context.Posts.Any(_draft.Id))
            DeletePost();

         SaveChanges();
         return SaveResult.Success(_draft.Id, string.Empty);
      }


      private void DeletePost()
      {
         var post = _context.Posts.Single(x => x.Id == _draft.Id);
         _context.Posts.Remove(post);
      }

      private SaveResult PublishPost()
      {
         try
         {
            var post = _draft.ToPost(_dateProvider, _processor);
            _context.AddOrUpdate(post);
            SaveChanges();
            return SaveResult.Success(post.Id, post.Url);
         }
         catch (ServiceDependencyException ex)
         {
            SaveChanges();
            return SaveResult.Failure(_draft.Id, $"Draft saved but couldn't publish. {ex.Message}.");
         }
      }

      private void SaveChanges()
      {
         _context.SaveChanges();
         _imageContext.SaveChanges(_oldPostDirectory, _draft.Slugify(), _images);
      }

      public void Dispose() =>
         _context.Dispose();
   }
}
