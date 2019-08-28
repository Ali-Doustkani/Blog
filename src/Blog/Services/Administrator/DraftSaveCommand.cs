//using AutoMapper;
//using Blog.Domain;
//using Blog.Domain.Blogging;
//using Blog.Storage;
//using Blog.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Blog.Services.Administrator
//{
//   public class DraftSaveCommand : IDisposable
//   {
//      public DraftSaveCommand(BlogContext context,
//         DraftValidator validator,
//         ImageContext imageContext,
//         IHtmlProcessor processor,
//         IDateProvider dateProvider,
//         IStorageState storageState)
//      {
//         _context = context;
//         _validator = validator;
//         _imageContext = imageContext;
//         _processor = processor;
//         _dateProvider = dateProvider;
//         _storageState = storageState;
//      }

//      private readonly BlogContext _context;
//      private readonly DraftValidator _validator;
//      private readonly ImageContext _imageContext;
//      private readonly IHtmlProcessor _processor;
//      private readonly IDateProvider _dateProvider;
//      private Draft _draft;
//      private IStorageState _storageState;

//      public SaveResult Run(DraftEntry viewModel)
//      {
//         if (viewModel.Id == 0)
//         {
//            _draft = new Draft();
//            _context.Drafts.Add(_draft);
//         }
//         else
//         {
//            _draft = _context.Drafts.Find(viewModel.Id);
//         }

//         var command = new DraftUpdateCommand
//         {
//            Content = viewModel.Content,
//            EnglishUrl = viewModel.EnglishUrl,
//            Language = viewModel.Language,
//            Summary = viewModel.Summary,
//            Tags = viewModel.Tags,
//            Title = viewModel.Title
//         };
//         _draft.Update(command, _storageState);

//         //var validationResult = _validator.Validate(_draft);
//         //if (validationResult.Any())
//         //   return SaveResult.Failure(_draft.Id, validationResult);


//         if (viewModel.Publish)
//         {
//            return PublishPost();
//         }

//         if (_context.Posts.Any(_draft.Id))
//            DeletePost();

//         SaveChanges();
//         return SaveResult.Success(_draft.Id, string.Empty);
//      }


//      private void DeletePost()
//      {
//         var post = _context.Posts.Single(x => x.Id == _draft.Id);
//         _context.Posts.Remove(post);
//      }

//      private SaveResult PublishPost()
//      {
//         try
//         {
//            var post = _draft.Publish(_dateProvider, _processor,null);
//            _context.AddOrUpdate(post);
//            SaveChanges();
//            return SaveResult.Success(post.Id, post.Url);
//         }
//         catch (ServiceDependencyException ex)
//         {
//            SaveChanges();
//            return SaveResult.Failure(_draft.Id, $"Draft saved but couldn't publish. {ex.Message}.");
//         }
//      }

//      private void SaveChanges()
//      {
//         _context.SaveChanges();
//         _imageContext.SaveChanges();
//      }

//      public void Dispose() =>
//         _context.Dispose();
//   }
//}
