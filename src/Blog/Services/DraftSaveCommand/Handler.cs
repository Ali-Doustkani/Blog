using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.DraftSaveCommand
{
   public class Handler : IRequestHandler<DraftSaveCommand, Result>
   {
      public Handler(BlogContext context,
         ImageContext imageContext,
         IMapper mapper,
         IDateProvider dateProvider,
         IHtmlProcessor htmlProcessor)
      {
         _context = context;
         _imageContext = imageContext;
         _mapper = mapper;
         _dateProvider = dateProvider;
         _htmlProcessor = htmlProcessor;
      }

      private readonly BlogContext _context;
      private readonly ImageContext _imageContext;
      private readonly IMapper _mapper;
      private readonly IDateProvider _dateProvider;
      private readonly IHtmlProcessor _htmlProcessor;

      public async Task<Result> Handle(DraftSaveCommand request, CancellationToken cancellationToken)
      {
         if (await _context.Drafts.AnyAsync(x => x.Id != request.Id && x.Title == request.Title))
            return Result.MakeFailure($"A draft or post with title '{request.Title}' already exists");

         var draft = request.Id == 0
            ? new Draft()
            : await _context.GetDraft(request.Id);

         _context.Update(draft);

         var command = _mapper.Map<DraftUpdateCommand>(request);
         var updateResult = draft.Update(command);
         if (updateResult.Failed)
            return Result.MakeFailure(updateResult);

         await _imageContext.AddOrUpdateAsync(updateResult.Images);

         Result ret = Result.MakeSuccess();
         if (request.Publish)
         {
            var result = await draft.Publish(_dateProvider, _htmlProcessor);
            ret = result.Failed
               ? Result.MakeFailure(result)
               : Result.MakeSuccess(draft.Post.Url);
         }

         if (!request.Publish && draft.Post != null)
            draft.Unpublish();

         await _context.SaveChangesAsync();
         return ret;
      }
   }
}
