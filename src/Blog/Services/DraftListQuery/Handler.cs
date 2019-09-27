using Blog.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.DraftListQuery
{
   public class Handler : IRequestHandler<DraftListQuery, IEnumerable<DraftItem>>
   {
      public Handler(BlogContext context) =>
         _context = context;

      private readonly BlogContext _context;

      public async Task<IEnumerable<DraftItem>> Handle(DraftListQuery request, CancellationToken cancellationToken) =>
         await _context
         .Drafts
         .Select(x => new DraftItem
         {
            Id = x.Id,
            Title = x.Title,
            Published = x.Post != null
         })
         .ToArrayAsync();
   }
}