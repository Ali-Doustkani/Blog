﻿using System.Collections.Generic;

namespace Blog.ViewModels.Home
{
    public class PostRow
    {
        public string Title { get; set; }
        public string UrlTitle { get; set; }
        public string Date { get; set; }
        public string Summary { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
