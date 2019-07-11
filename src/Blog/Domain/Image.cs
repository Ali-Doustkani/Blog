using Blog.Utils;
using HtmlAgilityPack;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Blog.Domain
{
    public class Image
    {
        public Image(string filename, string postSlug, byte[] data)
        {
            Filename = Its.NotNull(filename);
            PostSlug = Its.NotNull(postSlug);
            Data = data;
        }

        public Image(string filename, string postSlug)
            : this(filename, postSlug, null)
        { }

        public string Filename { get; }
        public string PostSlug { get; }
        public string RelativePath { get => Path.Combine(Path.DirectorySeparatorChar.ToString(), "images", "posts", PostSlug, Filename); }
        public bool IsFile { get => Data == null; }
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
