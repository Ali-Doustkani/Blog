using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Infrastructure;
using Blog.Utils;
using MediatR;

namespace Blog.Services.DraftSaveCommand
{
   public class Handler : RequestHandler<DraftSaveCommand, Result>
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

         var images = draft.Update(command);

         _context.SaveChanges();
         _imageContext.AddOrUpdate(images);

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
