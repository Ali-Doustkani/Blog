using AutoMapper;
using Blog.Domain;
using Blog.Domain.DeveloperStory;
using Blog.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Services.DeveloperSaveCommand
{
   public class Handler : IRequestHandler<DeveloperSaveCommand, DeveloperSaveResult>
   {
      public Handler(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      public async Task<DeveloperSaveResult> Handle(DeveloperSaveCommand request, CancellationToken cancellationToken)
      {
         var command = _mapper.Map<DeveloperUpdateCommand>(request);
         if (await _context.Developers.AnyAsync())
         {
            var developer = await _context.GetDeveloper();
            developer.Updating += Developer_Updating;
            var result = developer.Update(command);
            developer.Updating -= Developer_Updating;
            if (result.Failed)
               return DeveloperSaveResult.MakeFailure(result.Errors);
            _context.Update(developer);
            await _context.SaveChangesAsync();
            return DeveloperSaveResult.MakeSuccess(false,
               developer.Experiences.Select(x => x.Id),
               developer.SideProjects.Select(x => x.Id),
               developer.Educations.Select(x => x.Id));
         }
         else
         {
            var result = Developer.Create(command);
            if (result.Failed)
               return DeveloperSaveResult.MakeFailure(result.Errors);
            _context.Update(result.Developer);
            await _context.SaveChangesAsync();
            return DeveloperSaveResult.MakeSuccess(true,
               result.Developer.Experiences.Select(x => x.Id),
               result.Developer.SideProjects.Select(x => x.Id),
               result.Developer.Educations.Select(x => x.Id));
         }
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
   }
}
