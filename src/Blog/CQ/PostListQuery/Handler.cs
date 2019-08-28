using AutoMapper;
using Blog.Storage;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Blog.CQ.PostListQuery
{
   public class Handler : RequestHandler<PostListQuery, IEnumerable<PostItem>>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      protected override IEnumerable<PostItem> Handle(PostListQuery request) =>
         _context.Posts
            .Where(x => x.Language == request.Language)
            .Select(_mapper.Map<PostItem>);
   }
}
