using AutoMapper;
using Blog.Domain;
using Blog.ViewModels.Administrator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services
{
    public class AdministratorServices
    {
        public AdministratorServices(BlogContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private readonly BlogContext _context;
        private readonly IMapper _mapper;

        public PostEntry Create() =>
            new PostEntry { PublishDate = DateTime.Now };

        public IEnumerable<PostRow> GetPosts() =>
            _context
            .Posts
            .Select(x => new PostRow { Id = x.Id, Show = x.Show, Title = x.Title });

        public PostEntry Get(int id) =>
            _mapper.Map<PostEntry>(_context.Posts.Find(id));

        public string Save(PostEntry viewModel)
        {
            var post = _mapper.Map<Post>(viewModel);
            post.PopulateUrlTitle();
            post.DisplayContent = Article.Decorate(post.MarkedContent);

            if (post.Id == 0)
                _context.Posts.Add(post);
            else
            {
                _context.Posts.Attach(post);
                _context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            _context.SaveChanges();
            return post.UrlTitle;
        }

        public void Delete(int id)
        {
            var post = _context.Posts.Find(id);
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }
    }
}
