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
            .Drafts
            .Select(_mapper.Map<PostRow>);

        public PostEntry Get(int id) =>
            _mapper.Map<PostEntry>(_context.Drafts.Single(x => x.Id == id));

        public string Save(PostEntry viewModel)
        {
            var post = _mapper.Map<Draft>(viewModel);

            if (_context.Drafts.Any(x => x.Id != post.Id && string.Equals(x.Title, post.Title, StringComparison.OrdinalIgnoreCase)))
                throw new ValidationException(nameof(PostEntry.Title), "This title already exists in the database.");

            post.PopulateUrlTitle();
            post.Render();

            if (post.Id == 0)
            {
                _context.Drafts.Add(post);
            }
            else
            {
                _context.Drafts.Attach(post);
                _context.Entry(post).State = EntityState.Modified;
                _context.Entry(post).State = EntityState.Modified;
            }

            _context.SaveChanges();
            _imageContext.SaveChanges(post.GetImages());

            return post.UrlTitle;
        }

        public void Delete(int id)
        {
            var post = _context.Drafts.Find(id);
            _context.Drafts.Remove(post);
            _context.SaveChanges();
            _imageContext.Delete(post.UrlTitle);
        }
    }
}
