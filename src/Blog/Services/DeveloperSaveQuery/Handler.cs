using AutoMapper;
using Blog.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.DeveloperSaveQuery
{
   public class Handler : IRequestHandler<DeveloperSaveQuery, DeveloperSaveCommand.DeveloperSaveCommand>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      public async Task<DeveloperSaveCommand.DeveloperSaveCommand> Handle(DeveloperSaveQuery request, CancellationToken cancellationToken) =>
         _mapper.Map<DeveloperSaveCommand.DeveloperSaveCommand>(await _context.GetDeveloper());
   }
}
