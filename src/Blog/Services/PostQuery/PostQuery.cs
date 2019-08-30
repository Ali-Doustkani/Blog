using MediatR;

namespace Blog.Services.PostQuery
{
   public class PostQuery : IRequest<PostViewModel>
   {
      public string PostUrl { get; set; }
   }
}
