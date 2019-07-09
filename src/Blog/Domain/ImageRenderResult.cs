using System.Collections.Generic;

namespace Blog.Domain
{
    public class ImageRenderResult
    {
        public ImageRenderResult(string html, IEnumerable<Image> images)
        {
            Html = html;
            Images = images;
        }

        public string Html { get; }
        public IEnumerable<Image> Images { get; }
    }
}
