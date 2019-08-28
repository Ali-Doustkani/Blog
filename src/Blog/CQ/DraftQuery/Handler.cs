using AutoMapper;
using Blog.Storage;
using MediatR;

namespace Blog.CQ.DraftQuery
{
   public class Handler : RequestHandler<DraftQuery, DraftSaveCommand.DraftSaveCommand>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      protected override DraftSaveCommand.DraftSaveCommand Handle(DraftQuery request)
      {
         return _mapper.Map<DraftSaveCommand.DraftSaveCommand>(_context.GetDraft(request.Id));
      }
   }
}
