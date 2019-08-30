using Blog.Storage;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.DraftListQuery
{
   public class Handler : RequestHandler<DraftListQuery, IEnumerable<DraftItem>>
   {
      public Handler(BlogContext context) =>
         _context = context;

      private readonly BlogContext _context;

      protected override IEnumerable<DraftItem> Handle(DraftListQuery request) =>
         _context
         .Drafts
         .Select(x => new DraftItem
         {
            Id = x.Id,
            Title = x.Title,
            Published = x.Post != null
         })
         .ToArray();
   }
}