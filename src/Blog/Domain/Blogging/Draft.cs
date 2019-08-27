using Blog.Utils;
using System;
using System.Collections.Generic;

namespace Blog.Domain.Blogging
{
   public class Draft : DomainEntity
   {
      public Draft()
      {
         Language = Language.English;
      }

      private DateTime? _publishDate;

      public string Title { get; set; }

      public string EnglishUrl { get; set; }

      public Language Language { get; set; }

      public string Summary { get; set; }

      public string Tags { get; set; }

      public string Slugify()
      {
         if (Language == Language.English)
         {
            if (!string.IsNullOrEmpty(EnglishUrl))
               return EnglishUrl;

            return SlugifyTitle();
         }

         if (string.IsNullOrEmpty(EnglishUrl))
            throw new InvalidOperationException("Enlish URL of Farsi posts must have value");
         return EnglishUrl;
      }

      private string SlugifyTitle() =>
         Title
         .ThrowIfNullOrEmpty<InvalidOperationException>("Publishing needs Title to be set.")
         .ToLower()
         .Replace("#", "sharp")
         .ReplaceWithPattern(@"[\s:/\\]+", "-")
         .ReplaceWithPattern(@"\.", "");

      public string Content { get; set; }

      public IEnumerable<Image> RenderImages()
      {
         var renderer = new ImageRenderer(Slugify());
         var result = renderer.Render(Content);
         Content = result.Html;
         return result.Images;
      }

      /// <exception cref="ServiceDependencyException"/>
      public Post ToPost(IDateProvider dateProvider, IHtmlProcessor processor)
      {
         Assert.Op.NotNull(Title);
         Assert.Op.NotNull(Tags);
         Assert.Op.NotNull(Summary);
         Assert.Op.NotNull(Content);

         if (!_publishDate.HasValue)
            _publishDate = dateProvider.Now;

         return new Post(Id,
              Title,
              _publishDate.Value,
              Language,
              Summary,
              Tags,
              Slugify(),
              processor.Process(Content));
      }
   }
}
