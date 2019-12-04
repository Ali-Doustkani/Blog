using Blog.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.DraftQuery
{
   public class Handler : IRequestHandler<DraftQuery, Draft>
   {
      public Handler(BlogContext context) =>
         _context = context;

      private readonly BlogContext _context;

      public Task<Draft> Handle(DraftQuery request, CancellationToken cancellationToken) =>
         _context
         .Drafts
         .Select(x => new Draft
         {
            Id = x.Id,
            Language = x.Language.ToString().ToLower(),
            Title = x.Title,
            Content = x.Content
         })
         .SingleAsync(x => x.Id == request.Id);
   }
}
