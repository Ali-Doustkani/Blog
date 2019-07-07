using AutoMapper;
using Blog.Domain;
using Blog.ViewModels.Administrator;
using Microsoft.EntityFrameworkCore;
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
            .Select(_mapper.Map<PostRow>);

        public PostEntry Get(int id) =>
            _mapper.Map<PostEntry>(_context.Posts.Include(x => x.Content).Single(x => x.Id == id));

        public string Save(PostEntry viewModel)
        {
            var post = _mapper.Map<Post>(viewModel);

            if (_context.Posts.Any(x => x.Id != post.Id && string.Equals(x.Title, post.Title, StringComparison.OrdinalIgnoreCase)))
                throw new ValidationException(nameof(PostEntry.Title), "This title already exists in the database.");

            post.PopulateUrlTitle();
            new Article(null).Decorate(post);

            if (post.Id == 0)
            {
                _context.Posts.Add(post);
            }
            else
            {
                _context.Posts.Attach(post);
                _context.Entry(post).State = EntityState.Modified;
                _context.Entry(post.Content).State = EntityState.Modified;
            }
            _context.SaveChanges();
            return post.UrlTitle;
        }

        public void Delete(int id)
        {
            _context.Posts.Remove(_context.Posts.Find(id));
            _context.PostContents.Remove(_context.PostContents.Find(id));
            _context.SaveChanges();
        }
    }
}
