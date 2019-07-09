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
    public class SaveResult
    {
        public SaveResult(bool published, string url)
        {
            Published = published;
            Url = url;
        }

        public bool Published { get; }
        public string Url { get; }
    }

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

        public SaveResult Save(PostEntry viewModel)
        {
            SaveResult result;

            var draft = _mapper.Map<Draft>(viewModel);

            if (_context.Infos.Any(x => x.Id != draft.Id && string.Equals(x.Title, draft.Info.Title, StringComparison.OrdinalIgnoreCase)))
                throw new ValidationException(nameof(PostEntry.Title), "This title already exists in the database.");

            var images = draft.RenderImages();

            if (draft.Id == 0)
            {
                _context.Drafts.Add(draft);
            }
            else
            {
                _context.Drafts.Attach(draft);
                _context.Entry(draft).State = EntityState.Modified;
            }

            if (viewModel.Show)
            {
                var publish = draft.Publish();
                if (_context.Posts.Any(x => x.Id == publish.Id))
                {
                    _context.Posts.Attach(publish);
                    _context.Entry(publish).State = EntityState.Modified;
                }
                else
                {
                    _context.Posts.Add(publish);
                }

                result = new SaveResult(true, publish.Url);
            }
            else
            {
                if (_context.Posts.Any(x => x.Id == draft.Id))
                {
                    _context.Posts.Remove(_context.Posts.Find(draft.Id));
                }

                result = new SaveResult(false, string.Empty);
            }

            _context.SaveChanges();
            _imageContext.SaveChanges(images);

            return result;
        }

        public void Delete(int id)
        {
            var post = _context.Drafts.Find(id);
            _context.Drafts.Remove(post);
            _context.SaveChanges();
            // _imageContext.Delete(post.UrlTitle);
        }
    }
}
