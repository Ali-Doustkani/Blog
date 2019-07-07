using AutoMapper;
using Blog.Domain;
using Blog.ViewModels.Home;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services
{
    public class HomeServices
    {
        public HomeServices(BlogContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private readonly BlogContext _context;
        private readonly IMapper _mapper;

        public PostViewModel Get(string urlTitle) =>
            _mapper.Map<PostViewModel>(_context.Posts.SingleOrDefault(x => x.UrlTitle == urlTitle));

        public IEnumerable<PostRow> GetPosts(Language language) =>
          _context.
          Posts.
          Where(x => x.Language == language).
          Select(_mapper.Map<PostRow>);

        public IEnumerable<PostRow> GetVerifiedPosts(Language language) =>
            _context.
            Posts.
            Where(x => x.Show && x.Language == language).
            Select(_mapper.Map<PostRow>);
    }
}