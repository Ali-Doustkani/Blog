using AutoMapper;
using Blog.Services.PostQuery;
using Blog.Domain;
using Blog.Domain.Blogging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.DraftPreviewQuery
{
   public class Handler : IRequestHandler<DraftPreviewQuery, Result>
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

      public async Task<Result> Handle(DraftPreviewQuery request, CancellationToken cancellationToken)
      {
         var draft = new Draft();
         var updateInfo = _mapper.Map<DraftUpdateCommand>(request);
         updateInfo.Summary = "Not Important";
         var updateResult = draft.Update(updateInfo);
         if (updateResult.Failed)
            return Result.MakeFailure(updateResult);

         var publishResult = await draft.Publish(_dateProvider, _htmlProcessor);
         if (publishResult.Failed)
            return Result.MakeFailure(publishResult);

         return Result.MakeSuccess(_mapper.Map<PostViewModel>(draft.Post));
      }
   }
}
