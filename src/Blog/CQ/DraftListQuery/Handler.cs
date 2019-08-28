using AutoMapper;
using Blog.Storage;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.CQ.DraftListQuery
{
   public class Handler : RequestHandler<DraftListQuery, IEnumerable<DraftItem>>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      protected override IEnumerable<DraftItem> Handle(DraftListQuery request)
      {
         return (from info in _context.Drafts
                 join post in _context.Posts on info.Id equals post.Id into posts
                 from post in posts.DefaultIfEmpty()
                 select _mapper.Map<DraftItem>(Tuple.Create(info, post == null ? -1 : post.Id))
            ).ToArray();
      }
   }
}

