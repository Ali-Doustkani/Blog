using Blog.Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Blog.Utils
{
    public interface IImageContext
    {
        void SaveChanges(string postSlug, IEnumerable<Image> images);
        void Delete(string urlTitle);
    }

    public class ImageContext : IImageContext
    {
        public ImageContext(IFileSystem fs)
        {
            _fs = fs;
        }

        private readonly IFileSystem _fs;

        public void SaveChanges(string postSlug, IEnumerable<Image> images)
        {
            var directory = GetDirectory(postSlug);
            CreteDirectory(directory, images);
            WriteImages(directory, images);
            DeleteOrphanFiles(directory, images);
        }

        private void CreteDirectory(string directory, IEnumerable<Image> images)
        {
            if (images.Any())
                _fs.CreateDirectory(directory);
        }

        private void WriteImages(string directory, IEnumerable<Image> images)
        {
            foreach (var image in images)
            {
                if (!image.IsFile)
                    _fs.WriteAllBytes(Path.Combine(directory, image.Filename), image.Data);
            }
        }

        private void DeleteOrphanFiles(string directory, IEnumerable<Image> images)
        {
            foreach (var file in _fs.GetFiles(directory))
            {
                if (!images.Any(x => x.Filename == Path.GetFileName(file)))
                    _fs.DeleteFile(file);
            }

            if (!_fs.GetFiles(directory).Any())
                _fs.DeleteDirectory(directory);
        }

        public void Delete(string postSlug)
        {
            var directory = GetDirectory(postSlug);
            if (_fs.DirectoryExists(directory))
                _fs.DeleteDirectory(directory);
        }

        private string GetDirectory(string postSlug) =>
            Path.Combine("wwwroot", "images", "posts", postSlug);
    }
}
