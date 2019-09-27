using AutoMapper;
using Blog.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.PostQuery
{
   public class Handler : IRequestHandler<PostQuery, PostViewModel>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      public async Task<PostViewModel> Handle(PostQuery request, CancellationToken cancellationToken) =>
         _mapper.Map<PostViewModel>(await _context
            .Posts
            .SingleOrDefaultAsync(x => x.Url == request.PostUrl));
   }
}
