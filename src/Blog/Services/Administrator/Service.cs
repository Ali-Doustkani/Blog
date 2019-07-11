using AutoMapper;
using Blog.Domain;
using Blog.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.Administrator
{
    public class Service
    {
        public Service(BlogContext context, IMapper mapper, IImageContext imageContext)
        {
            _context = context;
            _mapper = mapper;
            _imageContext = imageContext;
        }

        private readonly BlogContext _context;
        private readonly IMapper _mapper;
        private readonly IImageContext _imageContext;

        public DraftEntry Create() =>
            new DraftEntry { PublishDate = DateTime.Now };

        public IEnumerable<DraftRow> GetDrafts() =>
            (from info in _context.Infos
             join post in _context.Posts on info.Id equals post.Id into posts
             from post in posts.DefaultIfEmpty()
             select _mapper.Map<DraftRow>(Tuple.Create(info, post == null ? -1 : post.Id))
             ).ToArray();

        public DraftEntry Get(int id) =>
            (from draft in _context.Drafts.Include(x => x.Info)
             join post in _context.Posts on draft.Id equals post.Id into posts
             from post in posts.DefaultIfEmpty()
             where draft.Id == id
             select _mapper.Map<DraftEntry>(Tuple.Create(draft, post == null ? -1 : post.Id))
             ).Single();

        public string Save(DraftEntry viewModel)
        {
            var result = string.Empty;

            var draft = _mapper.Map<Draft>(viewModel);

            if (_context.Infos.Any(x => x.Id != draft.Id && string.Equals(x.Title, draft.Info.Title, StringComparison.OrdinalIgnoreCase)))
                throw new ValidationException(nameof(DraftEntry.Title), "This title already exists in the database.");

            var images = draft.RenderImages();
            _context.AddOrUpdate(draft);

            if (viewModel.Publish)
            {
                var post = draft.Publish();
                _context.AddOrUpdate(post);
                result = post.Url;
            }
            else if (_context.Posts.Any(draft.Id))
            {
                _context.Posts.Delete(draft.Id);
                _context.PostContents.Delete(draft.Id);
            }

            _context.SaveChanges();
            _imageContext.SaveChanges(images);

            return result;
        }

        public void Delete(int id)
        {
            var info = _context.Infos.Find(id);
            _context.Drafts.Delete(id);
            _context.SaveChanges();
            _imageContext.Delete(info.Slugify());
        }
    }
}
