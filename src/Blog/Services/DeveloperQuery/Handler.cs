using AutoMapper;
using Blog.Infrastructure;
using MediatR;

namespace Blog.Services.DeveloperQuery
{
   public class Handler : RequestHandler<DeveloperQuery, DeveloperViewModel>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      protected override DeveloperViewModel Handle(DeveloperQuery request) =>
         _mapper.Map<DeveloperViewModel>(_context.GetDeveloper());
   }
}
