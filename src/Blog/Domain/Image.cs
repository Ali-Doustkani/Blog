using Blog.Utils;
using HtmlAgilityPack;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Blog.Domain
{
    public class Image
    {
        public Image(string fullname, byte[] data)
        {
            Fullname = Its.NotNull(fullname);
            Data = Its.NotNull(data);
        }

        public string Filename { get => Path.GetFileName(Fullname); }
        public string Fullname { get; }
        public byte[] Data { get; }

        public static Image Create(HtmlNode img, string imagePath)
        {
            Its.NotNull(img, nameof(img));

            if (!img.Attributes.Contains("src"))
                throw new InvalidOperationException("<img> must have src attribute in order to read the image.");

            var src = img.Attr("src");
            if (!IsDataUrl(src))
                throw new InvalidOperationException($"src must have the Data URL pattern. DataURL: {src}");

            var url = Regex.Match(src, @",(?<data>.*)");
            var base64Data = url.Groups["data"].Value;
            return new Image(imagePath, Convert.FromBase64String(base64Data));
        }

        public static bool IsDataUrl(string src) =>
              Regex.IsMatch(src, @"^data:image/(?:[a-zA-Z]+);base64,[a-zA-Z0-9+/]+=*$");

    }
}
