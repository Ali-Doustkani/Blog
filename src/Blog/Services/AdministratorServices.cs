using AutoMapper;
using Blog.Domain;
using Blog.Utils;
using Blog.ViewModels.Administrator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services
{
    public class AdministratorServices
    {
        public AdministratorServices(BlogContext context, IMapper mapper, IImageContext imageContext)
        {
            _context = context;
            _mapper = mapper;
            _imageContext = imageContext;
        }

        private readonly BlogContext _context;
        private readonly IMapper _mapper;
        private readonly IImageContext _imageContext;

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
            post.Content.Render();

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
            _imageContext.SaveChanges(post.Content.GetImages());

            return post.UrlTitle;
        }

        public void Delete(int id)
        {
            var post = _context.Posts.Find(id);
            _context.Posts.Remove(post);
            _context.PostContents.Remove(_context.PostContents.Find(id));
            _context.SaveChanges();
            _imageContext.Delete(post.UrlTitle);
        }
    }
}
