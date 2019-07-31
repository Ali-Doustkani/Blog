﻿using AutoMapper;
using Blog.Domain;
using Blog.Domain.DeveloperStory;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blog.Services.DeveloperStory
{
   public interface IDeveloperServices
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

      private Developer TheDeveloper() =>
         _context
            .Developers
            .Include(x => x.Experiences)
            .Include(x => x.SideProjects)
            .SingleOrDefault();
   }
}
