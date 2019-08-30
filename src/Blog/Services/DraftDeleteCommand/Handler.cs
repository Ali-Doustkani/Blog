using Blog.Storage;
using Blog.Utils;
using MediatR;

namespace Blog.Services.DraftDeleteCommand
{
   public class Handler : RequestHandler<DraftDeleteCommand>
   {
      public Handler(BlogContext context, ImageContext imageContext)
      {
         _context = context;
         _imageContext = imageContext;
      }

      private readonly BlogContext _context;
      private readonly ImageContext _imageContext;

      protected override void Handle(DraftDeleteCommand request)
      {
         var draft = _context.GetDraft(request.Id);
         _imageContext.Delete(draft.GetImageDirectoryName());
         _context.Drafts.Remove(draft);
         _context.SaveChanges();
      }
   }
}
