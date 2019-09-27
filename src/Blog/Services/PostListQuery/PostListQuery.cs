using Blog.Domain;
using MediatR;
using System.Collections.Generic;

namespace Blog.Services.PostListQuery
{
   public class PostListQuery : IRequest<IEnumerable<PostItem>>
   {
      public Language Language { get; set; }
   }
}
