using AutoMapper;
using Blog.Domain;
using Blog.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.Administrator
{
   public class DraftSaveCommand
   {
      public DraftSaveCommand(BlogContext context,
         IMapper mapper,
         DraftValidator validator,
         IImageContext imageContext,
         ICodeFormatter codeFormatter,
         IImageProcessor imageProcessor)
      {
         _context = context;
         _mapper = mapper;
         _validator = validator;
         _imageContext = imageContext;
         _codeFormatter = codeFormatter;
         _imageProcessor = imageProcessor;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;
      private readonly DraftValidator _validator;
      private readonly IImageContext _imageContext;
      private readonly ICodeFormatter _codeFormatter;
      private readonly IImageProcessor _imageProcessor;
      private Draft _draft;
      private IEnumerable<Image> _images;
      private string _oldPostDirectory;

      public SaveResult Run(DraftEntry viewModel)
      {
         _draft = _mapper.Map<Draft>(viewModel);

         var validationResult = _validator.Validate(_draft);
         if (validationResult.Any())
            return SaveResult.Failure(_draft.Id, validationResult);

         _images = _draft.RenderImages();

         AddDraft();

         if (viewModel.Publish)
            return PublishPost();

         if (_context.Posts.Any(_draft.Id))
            DeletePost();

         SaveChanges();
         return SaveResult.Success(_draft.Id, string.Empty);
      }

      private void AddDraft()
      {
         var oldDraft = _context.Drafts.Include(x => x.Info).SingleOrDefault(x => x.Id == _draft.Id);
         _oldPostDirectory = oldDraft?.Info?.Slugify();
         if (oldDraft == null)
         {
            _context.Drafts.Add(_draft);
         }
         else
         {
            _context.Entry(oldDraft).CurrentValues.SetValues(_draft);
            _context.Entry(oldDraft.Info).CurrentValues.SetValues(_draft.Info);
         }
      }

      private void DeletePost()
      {
         _context.Posts.Delete(_draft.Id);
         _context.PostContents.Delete(_draft.Id);
      }

      private SaveResult PublishPost()
      {
         SaveResult result;
         try
         {
            var post = _draft.Publish(_codeFormatter, _imageProcessor);
            _context.AddOrUpdate(post);
            result = SaveResult.Success(post.Id, post.Url);
         }
         catch (ServiceDependencyException ex)
         {
            result = SaveResult.Failure(_draft.Id, $"Draft saved but couldn't publish. {ex.Message}.");
         }
         SaveChanges();
         return result;
      }

      private void SaveChanges()
      {
         _context.SaveChanges();
         _imageContext.SaveChanges(_oldPostDirectory, _draft.Info.Slugify(), _images);
      }
   }
}
