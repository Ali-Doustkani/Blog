using AutoMapper;
using Blog.CQ.PostQuery;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Storage;
using MediatR;

namespace Blog.CQ.PreviewQuery
{
   public class Handler : RequestHandler<DraftPreviewQuery, PostViewModel>
   {
      public Handler(BlogContext context, IDateProvider dateProvider, IHtmlProcessor htmlProcessor, IMapper mapper)
      {
         _context = context;
         _dateProvider = dateProvider;
         _htmlProcessor = htmlProcessor;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IDateProvider _dateProvider;
      private readonly IHtmlProcessor _htmlProcessor;
      private readonly IMapper _mapper;

      protected override PostViewModel Handle(DraftPreviewQuery request)
      {
         var draft = _context.GetDraft(request.DraftId);
         if (draft == null)
            return null;

         draft.Publish(_dateProvider, _htmlProcessor);
         return _mapper.Map<PostViewModel>(draft.Post);
      }
   }
}
