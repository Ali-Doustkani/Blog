using AutoMapper;
using Blog.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.DeveloperQuery
{
   public class Handler : IRequestHandler<DeveloperQuery, DeveloperViewModel>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      public async Task<DeveloperViewModel> Handle(DeveloperQuery request, CancellationToken cancellationToken) =>
         _mapper.Map<DeveloperViewModel>(await _context.GetDeveloper());
   }
}
