using AutoMapper;
using Blog.Storage;
using MediatR;

namespace Blog.Services.DeveloperSaveQuery
{
   public class Handler : RequestHandler<DeveloperSaveQuery, DeveloperSaveCommand.DeveloperSaveCommand>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      protected override DeveloperSaveCommand.DeveloperSaveCommand Handle(DeveloperSaveQuery request) =>
         _mapper.Map<DeveloperSaveCommand.DeveloperSaveCommand>(_context.GetDeveloper());
   }
}
