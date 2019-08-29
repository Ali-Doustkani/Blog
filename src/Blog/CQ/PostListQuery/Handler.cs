using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Storage;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.CQ.PostListQuery
{
   public class Handler : RequestHandler<PostListQuery, IEnumerable<PostItem>>
   {
      public Handler(BlogContext context) =>
         _context = context;

      private readonly BlogContext _context;

      protected override IEnumerable<PostItem> Handle(PostListQuery request) =>
         _context
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
         .ToArray();

      private string ToShortPersianDate(DateTime date, Language language) =>
         language == Language.English ?
         date.ToString("MMM yyyy") :
         Post.ToShortPersianDate(date);
   }
}
