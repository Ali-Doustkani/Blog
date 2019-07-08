using Blog.Domain;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Blog.Utils
{
    public interface IImageContext
    {
        void SaveChanges(IEnumerable<Image> images);
        void Delete(string urlTitle);
    }

    public class ImageContext : IImageContext
    {
        public ImageContext(ILogger<ImageContext> log)
        {
            _log = log;
        }

        private readonly ILogger _log;

        public void SaveChanges(IEnumerable<Image> images)
        {
            if (images.Any())
            {
                var dirpath = Path.GetDirectoryName(PostPath.PostImageAbsolute(images.First().Fullname));
                Directory.CreateDirectory(dirpath);
                foreach (var file in Directory.GetFiles(dirpath))
                    File.Delete(file);
            }
            foreach (var image in images)
            {
                _log.LogInformation("Write Image. Filename: {0}, Data Length: {1}", image.Filename, image.Data.Length);
                File.WriteAllBytes(PostPath.PostImageAbsolute(image.Fullname), image.Data);
            }
        }

        public void Delete(string urlTitle) =>
            Directory.Delete(PostPath.PostImageAbsolute(urlTitle), true);
    }
}
