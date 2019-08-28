using Blog.Domain;
using Blog.Storage;
using Blog.Utils;
using MediatR;

namespace Blog.CQ.DraftDeleteCommand
{
   public class Handler : RequestHandler<DraftDeleteCommand>
   {
      public Handler(BlogContext context, ImageContext imageContext, IStorageState storageState)
      {
         _context = context;
         _imageContext = imageContext;
         _storageState = storageState;
      }

      private readonly BlogContext _context;
      private readonly ImageContext _imageContext;
      private readonly IStorageState _storageState;

      protected override void Handle(DraftDeleteCommand request)
      {
         var draft = _context.GetDraft(request.Id);
         draft.RemoveImages(_storageState);
         _context.Drafts.Remove(draft);

         //var post = _context.Posts.SingleOrDefault(x => x.Id == request.Id);
         //if (draft.Post != null)
         //{
         //   _context.Posts.Remove(post);
         //}

         _context.SaveChanges();
         _imageContext.SaveChanges();
      }
   }
}
