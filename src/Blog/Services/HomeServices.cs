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
                .Publishes
                .Include(x => x.Info)
                .SingleOrDefault(x => x.UrlTitle == urlTitle)
                );

        public IEnumerable<PostRow> GetPosts(Language language) =>
            _context
            .Infos
            .Join(_context.Publishes, info => info.Id, pub => pub.Id, (info, pub) => info)
            .Where(x => x.Language == language)
            .Select(MapRow);

        public IEnumerable<PostRow> GetVerifiedPosts(Language language) =>
            _context
            .Infos
            .Join(_context.Publishes, info => info.Id, pub => pub.Id, (info, pub) => info)
            .Where(x => x.Language == language)
            .Select(MapRow);

        private PostRow MapRow(PostInfo post)
        {
            var row = _mapper.Map<PostRow>(post);
            row.Date =
                post.Language == Language.English ?
                post.PublishDate.ToString("MMM yyyy") :
                post.GetShortPersianDate();
            return row;
        }

        private PostViewModel MapViewModel(Post post)
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