using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Domain
{
    public class Post
    {
        public int Id { get; set; }

        public PostInfo Info { get; set; }

        public string Url { get; set; }

        public string Content { get; set; }
    }
}
