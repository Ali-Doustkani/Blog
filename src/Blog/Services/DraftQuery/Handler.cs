using Blog.Infrastructure;
using MediatR;
using System.Linq;

namespace Blog.Services.DraftQuery
{
   public class Handler : RequestHandler<DraftQuery, DraftSaveCommand.DraftSaveCommand>
   {
      public Handler(BlogContext context) =>
         _context = context;

      private readonly BlogContext _context;

      protected override DraftSaveCommand.DraftSaveCommand Handle(DraftQuery request) =>
         _context
         .Drafts
         .Select(x => new DraftSaveCommand.DraftSaveCommand
         {
            Id = x.Id,
            Content = x.Content,
            EnglishUrl = x.EnglishUrl,
            Language = x.Language,
            Publish = x.Post != null,
            Summary = x.Summary,
            Tags = x.Tags,
            Title = x.Title
         })
         .Single(x => x.Id == request.Id);
   }
}
