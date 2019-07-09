using Blog.Domain;
using System.Collections.Generic;

namespace Blog.ViewModels.Home
{
    public class PostViewModel
    {
        public string Title { get; set; }
        public Language Language { get; set; }
        public string Date { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
