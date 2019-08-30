using Blog.CQ.PostQuery;
using Blog.Domain;
using MediatR;

namespace Blog.CQ.PreviewQuery
{
   public class DraftPreviewQuery : IRequest<PostViewModel>
   {
      public Language Language { get; set; }
      public string Title { get; set; }
      public string EnglishUrl { get; set; }
      public string Content { get; set; }
      public string Tags { get; set; }
   }
}
