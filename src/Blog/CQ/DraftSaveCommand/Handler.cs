using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Storage;
using Blog.Utils;
using MediatR;

namespace Blog.CQ.DraftSaveCommand
{
   public class Handler : RequestHandler<DraftSaveCommand, Result>
   {
      public Handler(BlogContext context,
         ImageContext imageContext,
         IMapper mapper,
         IStorageState storageState,
         IDateProvider dateProvider,
         IHtmlProcessor htmlProcessor)
      {
         _context = context;
         _imageContext = imageContext;
         _mapper = mapper;
         _storageState = storageState;
         _dateProvider = dateProvider;
         _htmlProcessor = htmlProcessor;
      }

      private readonly BlogContext _context;
      private readonly ImageContext _imageContext;
      private readonly IMapper _mapper;
      private readonly IStorageState _storageState;
      private readonly IDateProvider _dateProvider;
      private readonly IHtmlProcessor _htmlProcessor;

      protected override Result Handle(DraftSaveCommand request)
      {
         Draft draft;
         if (request.Id == 0)
         {
            draft = new Draft();
            _context.Drafts.Add(draft);
         }
         else
         {
            draft = _context.GetDraft(request.Id);
         }

         var command = _mapper.Map<DraftUpdateCommand>(request);

         draft.Update(command, _storageState);

         _context.SaveChanges();
         _imageContext.SaveChanges();

         if (request.Publish)
         {
            try
            {
               draft.Publish(_dateProvider, _htmlProcessor);
            }
            catch (ServiceDependencyException exc)
            {
               // return reason
               return new Result(null);
            }
            _context.SaveChanges();
            return new Result(draft.Post.Url);
         }

         if (!request.Publish && draft.Post != null)
         {
            draft.Unpublish();
            _context.SaveChanges();
         }

         return new Result(null);
      }
   }
}
