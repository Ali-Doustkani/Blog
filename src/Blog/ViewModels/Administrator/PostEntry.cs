using Blog.Domain;
using Blog.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Administrator
{
    public class PostEntry
    {
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
    }
}
