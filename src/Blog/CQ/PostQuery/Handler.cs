using AutoMapper;
using Blog.Storage;
using MediatR;

namespace Blog.CQ.PostQuery
{
   public class Handler : RequestHandler<PostQuery, DraftSaveCommand.DraftSaveCommand>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      protected override DraftSaveCommand.DraftSaveCommand Handle(PostQuery request)
      {
         return _mapper.Map<DraftSaveCommand.DraftSaveCommand>(_context.GetDraft(request.Id));
      }
   }
}
