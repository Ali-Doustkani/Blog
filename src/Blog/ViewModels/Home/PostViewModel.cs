using Blog.Domain;
using System;
using System.Collections.Generic;

namespace Blog.ViewModels.Home
{
    public class PostViewModel
    {
        public string Title { get; set; }
        public Language Language { get; set; }
        public DateTime PublishDate { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string LongPersianDate { get; set; }
        public bool Show { get; set; }
    }
}
