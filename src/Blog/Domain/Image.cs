using Blog.Utils;
using HtmlAgilityPack;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Blog.Domain
{
    public class Image
    {
        public Image(string filename, string post, byte[] data)
        {
            Filename = Its.NotNull(filename);
            Post = Its.NotNull(post);
            Data = Its.NotNull(data);
        }

        public string Filename { get; }
        public string Post { get; }
        public string AbsolutePath { get => Path.Combine("wwwroot", "images", "posts", Post, Filename); }
        public string RelativePath { get => Path.Combine(Path.DirectorySeparatorChar.ToString(), "images", "posts", Post, Filename); }
        public byte[] Data { get; }

        public static Image Create(HtmlNode img, string filename, string post)
        {
            Its.NotNull(img, nameof(img));

            if (!img.Attributes.Contains("src"))
                throw new InvalidOperationException("<img> must have src attribute in order to read the image.");

            var src = img.Attr("src");
            if (!IsDataUrl(src))
                throw new InvalidOperationException($"src must have the Data URL pattern. DataURL: {src}");

            var url = Regex.Match(src, @",(?<data>.*)");
            var base64Data = url.Groups["data"].Value;
            return new Image(filename, post, Convert.FromBase64String(base64Data));
        }

        public static bool IsDataUrl(string src) =>
              Regex.IsMatch(src, @"^data:image/(?:[a-zA-Z]+);base64,[a-zA-Z0-9+/]+=*$");

    }
}
