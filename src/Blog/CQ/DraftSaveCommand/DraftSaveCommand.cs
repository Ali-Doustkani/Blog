using Blog.Domain;
using MediatR;

namespace Blog.CQ.DraftSaveCommand
{
   public class DraftSaveCommand : IRequest<Result>
   {
      public int Id { get; set; }
      public Language Language { get; set; }
      public string Title { get; set; }
      public string EnglishUrl { get; set; }
      public string Content { get; set; }
      public string Summary { get; set; }
      public string Tags { get; set; }
      public bool Publish { get; set; }
   }
}
