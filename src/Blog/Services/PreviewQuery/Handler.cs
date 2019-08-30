using AutoMapper;
using Blog.Services.PostQuery;
using Blog.Domain;
using Blog.Domain.Blogging;
using MediatR;

namespace Blog.Services.PreviewQuery
{
   public class Handler : RequestHandler<DraftPreviewQuery, PostViewModel>
   {
      public Handler(IDateProvider dateProvider, IHtmlProcessor htmlProcessor, IMapper mapper)
      {
         _dateProvider = dateProvider;
         _htmlProcessor = htmlProcessor;
         _mapper = mapper;
      }

      private readonly IDateProvider _dateProvider;
      private readonly IHtmlProcessor _htmlProcessor;
      private readonly IMapper _mapper;

      protected override PostViewModel Handle(DraftPreviewQuery request)
      {
         var draft = new Draft();
         var updateInfo = _mapper.Map<DraftUpdateCommand>(request);
         updateInfo.Summary = "Not Important";
         draft.Update(updateInfo);
         draft.Publish(_dateProvider, _htmlProcessor);
         return _mapper.Map<PostViewModel>(draft.Post);
      }
   }
}
