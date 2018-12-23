using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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

        [Required]
        [MustBeUnique]
        public string Title { get; set; }

        public DateTime PublishDate { get; set; }

        public Language Language { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Tags { get; set; }

        public bool Show { get; set; }

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

        public string FarsiPublishDate
        {
            get { return PublishDate.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("fa")); }
        }
    }
}
