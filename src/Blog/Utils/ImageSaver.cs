using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Blog.Utils
{
    public interface IImageSaver
    {
        string Save(string directory, HtmlNode img);
    }

    public class ImageSaver : IImageSaver
    {
        public ImageSaver(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        private readonly IFileSystem _fileSystem;

        public string Save(string directory, HtmlNode img)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentNullException(nameof(directory));

            if (img == null)
                throw new ArgumentNullException(nameof(img));

            if (string.IsNullOrEmpty(img.Attributes["src"]?.Value))
                throw new ArgumentException("<img> must have src attribute in order to read the image.");

            if (!Regex.IsMatch(img.Attributes["src"].Value, @"^data:image/(?:[a-zA-Z]+);base64,[a-zA-Z0-9+/]+=?$"))
                throw new ArgumentException("src must have the Data URL pattern.");

            var filename = Filename(img);
            var path = Path.Combine("images", "posts", directory);
            _fileSystem.Write(
                Path.Combine("wwwroot", path, filename),
                Data(img));
            return Path.Combine(Path.DirectorySeparatorChar.ToString(), path, filename);
        }

        private byte[] Data(HtmlNode img)
        {
            var url = Regex.Match(
                img.Attributes["src"].Value,
                @",(?<data>.*)");
            var base64Data = url.Groups["data"].Value;
            return Convert.FromBase64String(base64Data);
        }

        private string Filename(HtmlNode img)
        {
            var src = img.Attributes["src"].Value;
            var extension =
                string.Concat(".",
                    Regex
                    .Match(src, @"data:image/(?<type>.*),")
                    .Groups["type"]
                    .Value
                    .Split(';')
                    .First());

            var dataFilename = img.Attributes["data-filename"]?.Value;
            if (!string.IsNullOrEmpty(dataFilename))
                return dataFilename + extension;

            return Path.ChangeExtension(Path.GetRandomFileName(), extension);
        }
    }
}
