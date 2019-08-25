using AutoMapper;
using Blog.Domain;
using Blog.Domain.DeveloperStory;
using System;
using System.Linq;

namespace Blog.Services.DeveloperStory
{
   public interface IDeveloperStoryServices : IDisposable
   {
      DeveloperUpdateCommand Get();
      SaveResult Save(DeveloperUpdateCommand developer);
   }

   public class DeveloperStoryServices : IDeveloperStoryServices
   {
      public DeveloperStoryServices(BlogContext context, IMapper mapper, IStorageState storageState)
      {
         _context = context;
         _mapper = mapper;
         _storageState = storageState;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;
      private readonly IStorageState _storageState;

      public DeveloperUpdateCommand Get() =>
         _mapper.Map<DeveloperUpdateCommand>(_context.GetDeveloper());

      public SaveResult Save(DeveloperUpdateCommand developerEntry)
      {
         // validate command before run

         try
         {
            if (_context.Developers.Any())
            {
               var developer = _context.GetDeveloper();
               developer.Update(developerEntry, _storageState);
               _context.Update(developer);
               _context.SaveChanges();
               return SaveResult.Updated(developer);
            }
            else
            {
               var developer = new Developer(developerEntry.Summary, developerEntry.Skills);
               developer.Update(developerEntry, _storageState);
               _context.Update(developer);
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
