﻿using Blog.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Services.Administrator
{
    public class DraftEntry
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime PublishDate { get; set; }

        public Language Language { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Tags { get; set; }

        public bool Publish { get; set; }
    }
}
