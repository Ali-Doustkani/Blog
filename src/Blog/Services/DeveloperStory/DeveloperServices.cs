using AutoMapper;
using Blog.Domain;
using Blog.Domain.DeveloperStory;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blog.Services.DeveloperStory
{
   public interface IDeveloperServices : IService
   {
      DeveloperEntry Get();
      SaveResult Save(DeveloperEntry developer);
   }

   public class DeveloperServices : IDeveloperServices
   {
      public DeveloperServices(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      public DeveloperEntry Get() =>
         _mapper.Map<DeveloperEntry>(TheDeveloper());

      public SaveResult Save(DeveloperEntry developerEntry)
      {
         try
         {
            var developer = _mapper.Map<Developer>(developerEntry);
            if (_context.Developers.Any())
            {
               var existing = TheDeveloper();
               developer.Id = existing.Id;
               _context.WriteOn(existing, developer, dev => dev.Experiences);
               _context.WriteOn(existing, developer, dev => dev.SideProjects);
               _context.SaveChanges();
               return SaveResult.Updated(developer);
            }
            _context.Developers.Add(developer);
            _context.SaveChanges();
            return SaveResult.Created(developer);
         }
         catch (DomainProblemException ex)
         {
            return SaveResult.Problematic(ex);
         }
         catch (AutoMapperMappingException ex)
         {
            if (ex.InnerException is DomainProblemException domainExc)
               return SaveResult.Problematic(domainExc);
            throw;
         }
      }

      private Developer TheDeveloper() =>
         _context
            .Developers
            .Include(x => x.Experiences)
            .Include(x => x.SideProjects)
            .SingleOrDefault();

      public void Dispose() =>
         _context.Dispose();
   }
}
