using Blog.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.DraftQuery
{
   public class Handler : IRequestHandler<DraftQuery, DraftSaveCommand.DraftSaveCommand>
   {
      public Handler(BlogContext context) =>
         _context = context;

      private readonly BlogContext _context;

      public Task<DraftSaveCommand.DraftSaveCommand> Handle(DraftQuery request, CancellationToken cancellationToken) =>
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
         .SingleAsync(x => x.Id == request.Id);
   }
}
