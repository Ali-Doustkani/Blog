using AutoMapper;
using Blog.Domain.Blogging;
using Blog.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.DraftSaveCommand
{
   public class Handler : IRequestHandler<DraftSaveCommand, DraftSaveResult>
   {
      public Handler(BlogContext context,
         ImageContext imageContext,
         IMapper mapper)
      {
         _context = context;
         _imageContext = imageContext;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly ImageContext _imageContext;
      private readonly IMapper _mapper;

      public async Task<DraftSaveResult> Handle(DraftSaveCommand request, CancellationToken cancellationToken)
      {
         if (await _context.Drafts.AnyAsync(x => x.Id != request.Id && x.Title == request.Title))
            return DraftSaveResult.MakeFailure(request.Id, $"A draft or post with title '{request.Title}' already exists");

         var draft = request.Id == 0
            ? new Draft()
            : await _context.GetDraft(request.Id);

         _context.Update(draft);

         var command = _mapper.Map<DraftUpdateCommand>(request);
         var updateResult = draft.Update(command);
         if (updateResult.Failed)
            return DraftSaveResult.MakeFailure(draft.Id, updateResult);

         await _imageContext.AddOrUpdateAsync(updateResult.Images);
         await _context.SaveChangesAsync();
         return DraftSaveResult.MakeSuccess(draft.Id);
      }
   }
}
