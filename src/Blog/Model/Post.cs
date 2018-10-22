using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Model
{
    public class Post
    {
        public Post()
        {
            Tags = string.Empty;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }

        public IEnumerable<string> TagCollection
        {
            get
            {
                if (string.IsNullOrEmpty(Tags))
                    return Enumerable.Empty<string>();

                var result = new List<string>();
                foreach (var str in Tags.Split(","))
                {
                    var trimmed = str.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                        result.Add(trimmed);
                }
                return result;
            }
        }
    }
}
