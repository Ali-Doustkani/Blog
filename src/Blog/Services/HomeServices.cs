using AutoMapper;
using Blog.Domain;
using Blog.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
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
            MapViewModel(_context
                .Drafts
                .Include(x => x.Info)
                .SingleOrDefault(x => x.UrlTitle == urlTitle)
                );

        public IEnumerable<PostRow> GetPosts(Language language) =>
            _context
            .Drafts
            .Include(x => x.Info)
            .Where(x => x.Info.Language == language)
            .Select(MapRow);

        public IEnumerable<PostRow> GetVerifiedPosts(Language language) =>
            _context
            .Drafts
            .Include(x => x.Info)
            .Where(x => x.Info.Language == language)
            .Select(MapRow);

        private PostRow MapRow(Draft post)
        {
            var row = _mapper.Map<PostRow>(post);
            row.Date =
                post.Info.Language == Language.English ?
                post.Info.PublishDate.ToString("MMM yyyy") :
                post.Info.GetShortPersianDate();
            return row;
        }

        private PostViewModel MapViewModel(Draft post)
        {
            var viewModel = _mapper.Map<PostViewModel>(post);
            viewModel.Date =
                post.Info.Language == Language.English ?
                post.Info.PublishDate.ToString("D") :
                post.Info.GetLongPersianDate();
            return viewModel;
        }
    }
}