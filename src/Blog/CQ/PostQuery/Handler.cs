using AutoMapper;
using Blog.Storage;
using MediatR;
using System.Linq;

namespace Blog.CQ.PostQuery
{
   public class Handler : RequestHandler<PostQuery, PostViewModel>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      protected override PostViewModel Handle(PostQuery request) =>
         _mapper.Map<PostViewModel>(_context
                .Posts
                .SingleOrDefault(x => x.Url == request.PostUrl));
   }
}
