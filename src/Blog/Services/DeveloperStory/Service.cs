using AutoMapper;
using Blog.Domain;
using Blog.Domain.DeveloperStory;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blog.Services.DeveloperStory
{
   public class Service
   {
      public Service(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      public DeveloperEntry GetDeveloper() =>
         _mapper.Map<DeveloperEntry>(TheDeveloper());

      public SaveResult SaveDeveloper(DeveloperEntry developerEntry)
      {
         var developer = _mapper.Map<Developer>(developerEntry);

         if (_context.Developers.Any())
         {
            var existing = TheDeveloper();
            developer.Id = existing.Id;
            _context.WriteOn(existing, developer, dev => dev.Experiences);
            _context.WriteOn(existing, developer, dev => dev.SideProjects);
            _context.SaveChanges();
            return MakeResult(existing);
         }

         _context.Developers.Add(developer);
         _context.SaveChanges();
         return MakeResult(developer);
      }

      private Developer TheDeveloper() =>
         _context
            .Developers
            .Include(x => x.Experiences)
            .Include(x => x.SideProjects)
            .SingleOrDefault();

      private SaveResult MakeResult(Developer developer) =>
         new SaveResult
         {
            Experiences = developer.Experiences.Select(x => x.Id),
            SideProjects = developer.SideProjects.Select(x => x.Id)
         };
   }
}
