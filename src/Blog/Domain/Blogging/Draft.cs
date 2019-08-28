using Blog.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain.Blogging
{
   public class DraftImages
   {
      public DraftImages(IEnumerable<Image> images, string oldDirectory, string newDirectory)
      {
         Images = images;
         OldDirectory = oldDirectory;
         NewDirectory = newDirectory;
      }

      public IEnumerable<Image> Images { get; }
      public string OldDirectory { get; }
      public string NewDirectory { get; }
   }

   public class DraftUpdateCommand
   {
      public int Id { get; set; }
      public Language Language { get; set; }
      public string Title { get; set; }
      public string EnglishUrl { get; set; }
      public string Content { get; set; }
      public string Summary { get; set; }
      public string Tags { get; set; }
   }

   public class PublishResult
   {
      public PublishResult(IEnumerable<Error> errors, Post post)
      {
         Errors = errors;
         Post = post;
      }

      public IEnumerable<Error> Errors { get; }

      public bool Failed => Errors.Any();

      public Post Post { get; }
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

      public string Slugify()
      {
         if (!string.IsNullOrEmpty(EnglishUrl))
            return EnglishUrl;
         return SlugifyTitle();
      }

      private string SlugifyTitle() =>
         Title
         .ThrowIfNullOrEmpty<InvalidOperationException>("Publishing needs Title to be set.")
         .ToLower()
         .Replace("#", "sharp")
         .ReplaceWithPattern(@"[\s:/\\]+", "-")
         .ReplaceWithPattern(@"\.", "");

      public IEnumerable<Image> RenderImages()
      {
         var renderer = new ImageRenderer(Slugify());
         var result = renderer.Render(Content);
         Content = result.Html;
         return result.Images;
      }

      /// <exception cref="ServiceDependencyException"/>
      public PublishResult Publish(IDateProvider dateProvider, IHtmlProcessor processor, IStorageState storageState)
      {
         Assert.Op.NotNull(Title);
         Assert.Op.NotNull(Tags);
         Assert.Op.NotNull(Summary);
         Assert.Op.NotNull(Content);

         if (Language == Language.Farsi && string.IsNullOrEmpty(EnglishUrl))
            return new PublishResult(new[] { new Error(nameof(EnglishUrl), "EnglishUrl is required for Farsi posts") }, null);

         Post post;
         if (!_publishDate.HasValue)
         {
            _publishDate = dateProvider.Now;
            post = ToPost(processor);
            storageState.Add(post);
         }
         else
         {
            post = ToPost(processor);
            storageState.Modify(post);
         }

         return new PublishResult(Enumerable.Empty<Error>(), post);
      }

      private Post ToPost(IHtmlProcessor processor)
      {
         return new Post(Id,
                Title,
                _publishDate.Value,
                Language,
                Summary,
                Tags,
                Slugify(),
                processor.Process(Content));
      }

      public void Update(DraftUpdateCommand command, IStorageState storageState)
      {
         Assert.Arg.NotNull(command);
         Assert.Arg.NotNull(storageState);

         var errors = ValidateCodeBlocks(command.Content);
         if (errors.Any())
            throw new InvalidOperationException("Content is not valid");

         var oldDirectory = Title == null ? null : Slugify();

         Title = Assert.Arg.NotNull(command.Title);
         Language = command.Language;
         EnglishUrl = command.EnglishUrl;
         Content = command.Content;
         Summary = command.Summary;
         Tags = command.Tags;

         storageState.Modify(new DraftImages(RenderImages(), oldDirectory, Slugify()));
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
   }
}
