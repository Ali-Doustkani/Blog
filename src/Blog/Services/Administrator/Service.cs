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

        public IEnumerable<DraftRow> GetDrafts()
        {
            var query = from info in _context.Infos
                        join draft in _context.Drafts on info.Id equals draft.Id
                        join post in _context.Posts on info.Id equals post.Id into posts
                        from post in posts.DefaultIfEmpty()
                        select _mapper.Map<DraftRow>(Tuple.Create(info, post == null ? -1 : post.Id));
            return query.ToArray();
        }

        public DraftEntry Get(int id) =>
            _mapper.Map<DraftEntry>(
                _context
                .Drafts
                .Include(x => x.Info)
                .Single(x => x.Id == id));

        public string Save(DraftEntry viewModel)
        {
            var result = string.Empty;

            var draft = _mapper.Map<Draft>(viewModel);

            if (_context.Infos.Any(x => x.Id != draft.Id && string.Equals(x.Title, draft.Info.Title, StringComparison.OrdinalIgnoreCase)))
                throw new ValidationException(nameof(DraftEntry.Title), "This title already exists in the database.");

            var images = draft.RenderImages();
            _context.AddOrUpdate(draft);

            if (viewModel.Show)
            {
                var post = draft.Publish();
                _context.AddOrUpdate(post);
                result = post.Url;
            }
            else
            {
                if (_context.Posts.Any(draft.Id))
                    _context.Posts.Delete(draft.Id);
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
            _imageContext.Delete(info.EncodeTitle());
        }
    }
}
