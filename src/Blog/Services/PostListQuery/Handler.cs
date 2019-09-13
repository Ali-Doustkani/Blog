using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.PostListQuery
{
   public class Handler : IRequestHandler<PostListQuery, IEnumerable<PostItem>>
   {
      public Handler(BlogContext context) =>
         _context = context;

      private readonly BlogContext _context;

      public async Task<IEnumerable<PostItem>> Handle(PostListQuery request, CancellationToken cancellationToken) =>
         await _context
          .Posts
          .Where(x => x.Language == request.Language)
          .Select(x => new PostItem
          {
             Date = ToShortPersianDate(x.PublishDate, x.Language),
             Summary = x.Summary,
             Tags = Post.ToTags(x.Tags),
             Title = x.Title,
             Url = x.Url
          })
          .ToArrayAsync();

      private string ToShortPersianDate(DateTime date, Language language) =>
         language == Language.English ?
         date.ToString("MMM yyyy") :
         Post.ToShortPersianDate(date);
   }
}
