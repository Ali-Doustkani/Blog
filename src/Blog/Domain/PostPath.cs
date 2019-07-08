using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Blog.Domain
{
    public static class PostPath
    {
        private static readonly string IMAGES = Path.Combine("images", "posts");

        public static string PostImageAbsolute(params string[] segments)
        {
            var arr = new List<string>() { "wwwroot", IMAGES };
            arr.AddRange(segments);
            return Path.Combine(arr.ToArray());
        }

        public static string ImageUrl(params string[] segments)
        {
            var param = segments.ToList();
            param.Insert(0, Path.DirectorySeparatorChar.ToString());
            param.Insert(1, IMAGES);
            return Path.Combine(param.ToArray());
        }

        public static string NewFileName(string extension) =>
             Path.ChangeExtension(Path.GetRandomFileName(), extension);
    }
}
