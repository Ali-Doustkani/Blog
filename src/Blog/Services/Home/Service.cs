using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.Home
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

      public PostViewModel Get(string urlTitle) =>
          _mapper.Map<PostViewModel>(_context
              .Posts
              .Include(x => x.Info)
              .Include(x => x.PostContent)
              .SingleOrDefault(x => x.Url == urlTitle)
              );

      public IEnumerable<PostRow> GetPosts(Language language) =>
          _context
          .Posts
          .Include(x => x.Info)
          .Where(x => x.Info.Language == language)
          .Select(_mapper.Map<PostRow>);
   }
}