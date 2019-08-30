using AutoMapper;
using Blog.Domain;
using Blog.Domain.DeveloperStory;
using Blog.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blog.Services.DeveloperSaveCommand
{
   public class Handler : RequestHandler<DeveloperSaveCommand, DeveloperSaveResult>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      protected override DeveloperSaveResult Handle(DeveloperSaveCommand request)
      {
         var command = _mapper.Map<DeveloperUpdateCommand>(request);
         var created = CreateOrGet(command, out Developer developer);

         developer.Updating += Developer_Updating;
         var result = developer.Update(command);
         developer.Updating -= Developer_Updating;

         _context.Update(developer);
         _context.SaveChanges();

         return new DeveloperSaveResult(created, result);
      }

      private void Developer_Updating(UpdatingType type, DomainEntity entity)
      {
         var state = type == UpdatingType.Removing ? EntityState.Detached : EntityState.Modified;
         if (entity is Experience exp)
            _context.Entry(exp.Period).State = state;
         else if (entity is Education edu)
            _context.Entry(edu.Period).State = state;
         _context.Entry(entity).State = state;
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
