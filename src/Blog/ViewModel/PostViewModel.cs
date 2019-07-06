using Blog.Domain;
using Blog.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Model
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required]
        [MustBeUnique]
        public string Title { get; set; }

        public string UrlTitle { get; set; }

        public DateTime PublishDate { get; set; }

        public Language Language { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string MarkedContent { get; set; }

        public string DisplayContent { get; set; }

        [Required]
        public string Tags { get; set; }

        public bool Show { get; set; }

        public IEnumerable<string> TagCollection { get; set; }

        public string LongPersianDate { get; set; }

        public string ShortPersianDate { get; set; }
    }
}
