using AutoMapper;
using Blog.Domain;
using Blog.Domain.DeveloperStory;
using Blog.Storage;
using MediatR;
using System.Linq;

namespace Blog.CQ.DeveloperSaveCommand
{
   public class Handler : RequestHandler<DeveloperSaveCommand, DeveloperSaveResult>
   {
      public Handler(BlogContext context, IStorageState storageState, IMapper mapper)
      {
         _context = context;
         _storageState = storageState;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IStorageState _storageState;
      private readonly IMapper _mapper;

      protected override DeveloperSaveResult Handle(DeveloperSaveCommand request)
      {
         var command = _mapper.Map<DeveloperUpdateCommand>(request);
         var created = CreateOrGet(command, out Developer developer);
         var result = developer.Update(command, _storageState);

         _context.Update(developer);
         _context.SaveChanges();

         return new DeveloperSaveResult(created, result);
      }

      private bool CreateOrGet(DeveloperUpdateCommand updateCommand, out Developer developer)
      {
         if (_context.Developers.Any())
         {
            developer = _context.GetDeveloper();
            return false;
         }

         var experiences = updateCommand
            .Experiences
            .Select(x => new Experience(0, x.Company, x.Position, Period.Parse(x.StartDate, x.EndDate), x.Content));
         developer = new Developer(updateCommand.Summary, updateCommand.Skills, experiences);
         return true;
      }
   }
}
