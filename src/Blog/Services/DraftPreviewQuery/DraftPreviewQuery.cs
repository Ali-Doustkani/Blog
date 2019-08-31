using Blog.Domain;
using MediatR;

namespace Blog.Services.DraftPreviewQuery
{
   public class DraftPreviewQuery : IRequest<Result>
   {
      public Language Language { get; set; }
      public string Title { get; set; }
      public string EnglishUrl { get; set; }
      public string Content { get; set; }
      public string Tags { get; set; }
   }
}
