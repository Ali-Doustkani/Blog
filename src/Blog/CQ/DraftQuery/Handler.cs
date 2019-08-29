using AutoMapper;
using Blog.Storage;
using MediatR;
using System.Linq;

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
         return _context.Drafts
            .Select(x => new DraftSaveCommand.DraftSaveCommand
            {
               Id = x.Id,
               Content = x.Content,
               EnglishUrl = x.EnglishUrl,
               Language = x.Language,
               Publish = x.Post != null,
               Summary = x.Summary,
               Tags = x.Tags,
               Title = x.Title
            }).Single(x => x.Id == request.Id);
         //return _mapper.Map<DraftSaveCommand.DraftSaveCommand>(_context.GetDraft(request.Id));
      }
   }
}
