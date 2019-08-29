using Blog.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain.Blogging
{
   public class DraftUpdateCommand
   {
      public Language Language { get; set; }
      public string Title { get; set; }
      public string EnglishUrl { get; set; }
      public string Content { get; set; }
      public string Summary { get; set; }
      public string Tags { get; set; }
   }

   public class Draft : DomainEntity
   {
      public Draft() { }

      public Draft(int id, string title, string englishUrl, Language language, string summary, string tags, string content)
      {
         Id = id;
         Title = title;
         EnglishUrl = englishUrl;
         Language = language;
         Summary = summary;
         Tags = tags;
         Content = content;
      }

      private static string[] languages = new[] { "csharp", "cs", "javascript", "js", "css", "sass", "less", "html", "sql" };

      private DateTime? _publishDate;

      public string Title { get; private set; }

      public string EnglishUrl { get; private set; }

      public Language Language { get; private set; }

      public string Summary { get; private set; }

      public string Tags { get; private set; }

      public string Content { get; private set; }

      public Post Post { get; private set; }

      public string GetImageDirectoryName()
      {
         if (!string.IsNullOrEmpty(EnglishUrl))
            return EnglishUrl;
         return SlugifyTitle();
      }

      /// <exception cref="ServiceDependencyException"/>
      /// <exception cref="InvalidOperationException" />
      public void Publish(IDateProvider dateProvider, IHtmlProcessor processor)
      {
         Assert.Op.NotNull(Title, "Title");
         Assert.Op.NotNull(Tags, "Tags");
         Assert.Op.NotNull(Summary, "Summary");
         Assert.Op.NotNull(Content, "Content");

         if (Language == Language.Farsi && string.IsNullOrEmpty(EnglishUrl))
            throw new InvalidOperationException("EnglishUrl is required for Farsi posts");

         if (!_publishDate.HasValue)
            _publishDate = dateProvider.Now;

         Post = new Post(Id,
                Title,
                _publishDate.Value,
                Language,
                Summary,
                Tags,
                GetImageDirectoryName(),
                processor.Process(Content));
      }

      public void Unpublish()
      {
         _publishDate = null;
         Post = null;
      }

      public ImageCollection Update(DraftUpdateCommand command)
      {
         Assert.Arg.NotNull(command);
         Assert.Op.NoError(ValidateCodeBlocks(command.Content));

         var oldDirectory = Title == null ? null : GetImageDirectoryName();

         Title = Assert.Arg.NotNull(command.Title);
         Language = command.Language;
         EnglishUrl = command.EnglishUrl;
         Content = command.Content ?? string.Empty;
         Summary = command.Summary;
         Tags = command.Tags;

         return RenderImages(oldDirectory);
      }

      public static IEnumerable<Error> ValidateCodeBlocks(string content)
      {
         var result = new List<Error>();

         if (string.IsNullOrEmpty(content))
            return result;

         var doc = new HtmlDocument();
         doc.LoadHtml(content);
         var num = 0;
         doc.DocumentNode.ForEachChild(node =>
         {
            var plain = node.InnerHtml.Trim();
            if (node.Is("pre.code") && !string.IsNullOrWhiteSpace(plain))
            {
               num++;

               if (!node.InnerHtml.Contains(Environment.NewLine))
               {
                  result.Add(new Error(nameof(content), $"Language is not specified for the code block #{num}"));
               }

               var lang = HtmlProcessor.GetLanguage(plain);
               if (!languages.Contains(lang))
                  result.Add(new Error(nameof(content), $"Specified language in code block #{num} is not valid ({lang}...)"));
            }
         });
         return result;
      }

      private string SlugifyTitle() =>
        Title
        .ThrowIfNullOrEmpty<InvalidOperationException>("Publishing needs Title to be set.")
        .ToLower()
        .Replace("#", "sharp")
        .ReplaceWithPattern(@"[\s:/\\]+", "-")
        .ReplaceWithPattern(@"\.", "");

      private ImageCollection RenderImages(string oldDirectory)
      {
         var renderer = new ImageRenderer(GetImageDirectoryName());
         var result = renderer.Render(Content);
         Content = result.Html;
         return new ImageCollection(result.Images, oldDirectory, GetImageDirectoryName());
      }
   }
}
