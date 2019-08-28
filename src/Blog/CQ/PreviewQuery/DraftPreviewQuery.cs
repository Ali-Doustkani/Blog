using Blog.Services.Home;
using MediatR;

namespace Blog.CQ.PreviewQuery
{
   public class DraftPreviewQuery : IRequest<PostViewModel>
   {
      public int DraftId { get; set; }
   }
}
