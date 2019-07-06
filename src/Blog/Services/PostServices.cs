using Blog.Domain;
using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services
{
    public class PostServices
    {
        public PostServices(BlogContext context)
        {
            _context = context;
        }

        private readonly BlogContext _context;

        public PostViewModel Create()
        {
            return new PostViewModel
            {
                PublishDate = DateTime.Now
            };
        }

        public string Save(PostViewModel viewModel)
        {
            var post = ToDomain(viewModel);
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

        public PostViewModel Get(int id) => ToViewModel(_context.Posts.Find(id));

        public PostViewModel Get(string urlTitle) => ToViewModel(_context.Posts.SingleOrDefault(x => x.UrlTitle == urlTitle));

        public IEnumerable<PostViewModel> GetPosts() =>
            _context.
            Posts.
            ToList().
            Select(ToViewModel);

        public IEnumerable<PostViewModel> GetPosts(Language language) =>
          _context.
          Posts.
          Where(x => x.Language == language).
          Select(ToViewModel);

        public IEnumerable<PostViewModel> GetVerifiedPosts(Language language) =>
            _context.
            Posts.
            Where(x => x.Show && x.Language == language).
            Select(ToViewModel);

        private Post ToDomain(PostViewModel viewModel) =>
            new Post
            {
                Language = viewModel.Language,
                DisplayContent = viewModel.DisplayContent,
                MarkedContent = viewModel.MarkedContent,
                PublishDate = viewModel.PublishDate,
                Show = viewModel.Show,
                Summary = viewModel.Summary,
                Tags = viewModel.Tags,
                Title = viewModel.Title
            };

        private PostViewModel ToViewModel(Post post) =>
            post == null ? null :
            new PostViewModel
            {
                DisplayContent = post.DisplayContent,
                Language = post.Language,
                MarkedContent = post.MarkedContent,
                PublishDate = post.PublishDate,
                Show = post.Show,
                Summary = post.Summary,
                Tags = post.Tags,
                TagCollection = post.TagCollection,
                Title = post.Title,
                UrlTitle = post.UrlTitle,
                LongPersianDate = post.GetLongPersianDate(),
                ShortPersianDate = post.GetShortPersianDate()
            };

    }
}
