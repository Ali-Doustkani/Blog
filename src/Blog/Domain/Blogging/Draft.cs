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

   public class DraftUpdateCommandResult : CommandResult
   {
      public DraftUpdateCommandResult(IEnumerable<string> errors)
         : base(errors)
      { }

      public DraftUpdateCommandResult(ImageCollection images)
          : base(Enumerable.Empty<string>())
      {
         Images = images;
      }

      public ImageCollection Images { get; }

      public static DraftUpdateCommandResult MakeFailure(IEnumerable<string> errors) =>
         new DraftUpdateCommandResult(errors);

      public static DraftUpdateCommandResult MakeSuccess(ImageCollection images) =>
         new DraftUpdateCommandResult(images);
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

      public CommandResult Publish(IDateProvider dateProvider, IHtmlProcessor processor)
      {
         var errors = new ErrorManager()
            .Required(Title, "Title")
            .Required(Tags, "Tags")
            .Required(Summary, "Summary")
            .Required(Content, "Content")
            .IfTrue(Language == Language.Farsi && string.IsNullOrEmpty(EnglishUrl), "EnglishUrl is required for Farsi posts")
            .Add(ValidateCodeBlocks());

         if (errors.Dirty)
            return errors.ToResult();

         if (!_publishDate.HasValue)
            _publishDate = dateProvider.Now;

         try
         {
            Post = new Post(Id,
                   Title,
                   _publishDate.Value,
                   Language,
                   Summary,
                   Tags,
                   GetImageDirectoryName(),
                   processor.Process(Content));
         }
         catch (ServiceDependencyException exc)
         {
            errors.Add(exc.Message);
         }

         return errors.ToResult();
      }

      public void Unpublish()
      {
         _publishDate = null;
         Post = null;
      }

      public DraftUpdateCommandResult Update(DraftUpdateCommand command)
      {
         var errorManager = new ErrorManager();
         errorManager.Required(command.Title, "Title");

         if (errorManager.Dirty)
            return DraftUpdateCommandResult.MakeFailure(errorManager.Errors);

         var oldDirectory = Title == null ? null : GetImageDirectoryName();

         Title = command.Title;
         Language = command.Language;
         EnglishUrl = command.EnglishUrl;
         Content = command.Content ?? string.Empty;
         Summary = command.Summary;
         Tags = command.Tags;

         return DraftUpdateCommandResult.MakeSuccess(RenderImages(oldDirectory));
      }

      private IEnumerable<string> ValidateCodeBlocks()
      {
         var result = new List<string>();

         if (string.IsNullOrEmpty(Content))
            return result;

         var doc = new HtmlDocument();
         doc.LoadHtml(Content);
         var num = 0;
         doc.DocumentNode.ForEachChild(node =>
         {
            var plain = node.InnerHtml.Trim();
            if (node.Is("pre.code") && !string.IsNullOrWhiteSpace(plain))
            {
               num++;

               if (!node.InnerHtml.Contains('\n'))
               {
                  result.Add($"Language is not specified for the code block #{num}");
               }

               var lang = HtmlProcessor.GetLanguage(plain);
               if (!languages.Contains(lang))
                  result.Add($"Specified language in code block #{num} is not valid ({lang}...)");
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
