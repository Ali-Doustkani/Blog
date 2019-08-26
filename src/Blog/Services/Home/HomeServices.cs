using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.Home
{
   public interface IHomeServices : IDisposable
   {
      PostViewModel Get(string urlTitle);
      IEnumerable<PostRow> GetPosts(Language language);
      DeveloperViewModel GetDeveloper();
   }

   public class HomeServices : IHomeServices
   {
      public HomeServices(BlogContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      private readonly BlogContext _context;
      private readonly IMapper _mapper;

      public PostViewModel Get(string urlTitle) =>
          _mapper.Map<PostViewModel>(_context
              .Posts
              .Include(x => x.PostContent)
              .SingleOrDefault(x => x.Url == urlTitle)
              );

      public IEnumerable<PostRow> GetPosts(Language language) =>
          _context
          .Posts
          .Where(x => x.Language == language)
          .Select(_mapper.Map<PostRow>);

      public DeveloperViewModel GetDeveloper() =>
         _mapper.Map<DeveloperViewModel>(_context.GetDeveloper());

      public void Dispose() =>
         _context.Dispose();

   }
}