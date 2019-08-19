using AutoMapper;
using Blog.Domain;
using Blog.Domain.DeveloperStory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
         _mapper.Map<DeveloperEntry>(TheDeveloper());

      public void UpdateList<TDomainEntity, TDomainItem, TDto>(TDomainEntity parent,
         IReadOnlyCollection<TDomainItem> entityCollection,
         IEnumerable<TDto> dtoCollection,
         Func<TDomainEntity, Action<TDomainItem>> remove,
         Func<TDomainItem, TDto, bool> equality
         )
      {
         foreach (var item in entityCollection)
         {
            if (!dtoCollection.Any(x => equality(item, x)))
            {
               remove(parent)(item);
            }
         }
      }

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
               var developer = TheDeveloper();
               developer.Update(developerEntry.Summary, developerEntry.Skills);

               UpdateList(
                  developer,
                  developer.Experiences,
                  developerEntry.Experiences,
                  d => d.RemoveExperience,
                  (experienceEntry, experience) => experience.Id == experienceEntry.Id.ToString()
                  );


               //foreach (var experience in developer.Experiences)
               //{
               //   if (!developerEntry.Experiences.Any(x => x.Id == experience.Id.ToString()))
               //   {
               //      developer.RemoveExperience(experience);
               //   }
               //}

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

               var toDel = developer
                  .SideProjects
                  .Where(x => !developerEntry.SideProjects.Any(y => y.Id == x.Id.ToString()))
                  .ToArray();
               foreach (var t in toDel)
                  developer.SideProjects.Remove(t);

               foreach (var side in developerEntry.SideProjects)
               {
                  if (int.TryParse(side.Id, out int id))
                  {
                     var toRemove = developer.SideProjects.Single(x => x.Id.ToString() == side.Id);
                     _context.Entry(toRemove).State = EntityState.Detached;
                     developer.SideProjects.Remove(toRemove);
                     var newSide = new SideProject(side.Title, side.Content);
                     newSide.Id = id;
                     developer.SideProjects.Add(newSide);
                     _context.Entry(newSide).State = EntityState.Modified;
                  }
                  else
                  {
                     developer.SideProjects.Add(new SideProject(side.Title, side.Content));
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
                  developer.SideProjects.Add(new SideProject(side.Title, side.Content));
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
