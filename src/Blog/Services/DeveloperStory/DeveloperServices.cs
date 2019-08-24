using AutoMapper;
using Blog.Domain;
using Blog.Domain.DeveloperStory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Blog.Services.DeveloperStory
{
   public interface IDeveloperServices : IDisposable
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
         _mapper.Map<DeveloperEntry>(_context.GetDeveloper());

      public SaveResult Save(DeveloperEntry developerEntry)
      {
         if (developerEntry.Experiences == null)
            developerEntry.Experiences = Enumerable.Empty<ExperienceEntry>();
         if (developerEntry.SideProjects == null)
            developerEntry.SideProjects = Enumerable.Empty<SideProjectEntry>();

         try
         {
            if (_context.Developers.Any())
            {
               var developer = _context.GetDeveloper();
               developer.Update(developerEntry.Summary, developerEntry.Skills);

               // experiences

               foreach (var experience in developer.Experiences)
               {
                  if (!developerEntry.Experiences.Any(x => x.Id == experience.Id.ToString()))
                  {
                     developer.RemoveExperience(experience);
                  }
               }

               foreach (var experience in developerEntry.Experiences)
               {
                  if (int.TryParse(experience.Id, out int id))
                  {
                     _context.Entry(developer.Experiences.Single(x => x.Id.ToString() == experience.Id)).State = EntityState.Detached;
                     developer.UpdateExperience(id,
                        experience.Company,
                        experience.Position,
                        DateTime.Parse(experience.StartDate),
                        DateTime.Parse(experience.EndDate),
                        experience.Content);
                     _context.Entry(developer.Experiences.Single(x => x.Id.ToString() == experience.Id)).State = EntityState.Modified;
                  }
                  else
                  {
                     developer.AddExperience(experience.Company,
                        experience.Position,
                        DateTime.Parse(experience.StartDate),
                        DateTime.Parse(experience.EndDate),
                        experience.Content);
                  }
               }

               // side projects

               foreach (var proj in developer.SideProjects)
               {
                  if (!developerEntry.SideProjects.Any(x => x.Id == proj.Id.ToString()))
                  {
                     developer.RemoveSideProject(proj);
                  }
               }

               foreach (var proj in developerEntry.SideProjects)
               {
                  if (int.TryParse(proj.Id, out int id))
                  {
                     _context.Entry(developer.SideProjects.Single(x => x.Id.ToString() == proj.Id)).State = EntityState.Detached;
                     developer.UpdateSideProject(id, proj.Title, proj.Content);
                     _context.Entry(developer.SideProjects.Single(x => x.Id.ToString() == proj.Id)).State = EntityState.Modified;
                  }
                  else
                  {
                     developer.AddSideProject(proj.Title, proj.Content);
                  }
               }

               _context.SaveChanges();
               return SaveResult.Updated(developer);
            }
            else
            {
               var developer = new Developer(developerEntry.Summary, developerEntry.Skills);
               foreach (var experience in developerEntry.Experiences)
               {
                  developer.AddExperience(experience.Company,
                     experience.Position,
                     DateTime.Parse(experience.StartDate),
                     DateTime.Parse(experience.EndDate),
                     experience.Content);
               }
               foreach (var side in developerEntry.SideProjects)
               {
                  developer.AddSideProject(side.Title, side.Content);
               }
               _context.Developers.Add(developer);
               _context.SaveChanges();
               return SaveResult.Created(developer);
            }
         }
         catch (DomainProblemException ex)
         {
            return SaveResult.Problematic(ex);
         }
      }

      public void Dispose() =>
         _context.Dispose();
   }
}
