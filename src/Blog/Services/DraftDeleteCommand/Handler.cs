using Blog.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.DraftDeleteCommand
{
   public class Handler : AsyncRequestHandler<DraftDeleteCommand>
   {
      public Handler(BlogContext context, ImageContext imageContext)
      {
         _context = context;
         _imageContext = imageContext;
      }

      private readonly BlogContext _context;
      private readonly ImageContext _imageContext;

      protected override async Task Handle(DraftDeleteCommand request, CancellationToken cancellationToken)
      {
         var draft = await _context.GetDraft(request.Id);
         _imageContext.Delete(draft.GetImageDirectoryName());
         _context.Drafts.Remove(draft);
         await _context.SaveChangesAsync();
      }
   }
}
