using MediatR;

namespace Blog.CQ.PostQuery
{
   public class PostQuery : IRequest<PostViewModel>
   {
      public string PostUrl { get; set; }
   }
}
