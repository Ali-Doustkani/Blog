using Blog.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain.Blogging
{
   public class DraftValidator
   {
      public DraftValidator(BlogContext context)
      {
         _context = Its.NotEmpty(context, nameof(context));
         _languages = new[] { "csharp", "cs", "javascript", "js", "css", "sass", "less", "html", "sql" };
      }

      private readonly BlogContext _context;
      private readonly string[] _languages;
      private List<Problem> _result;

      public IEnumerable<Problem> Validate(Draft draft)
      {
         _result = new List<Problem>();
         ValidateEnglishUrl(draft);
         ValidateTitle(draft);
         ValidateCodeBlocks(draft);
         return _result;
      }

      private void ValidateEnglishUrl(Draft draft)
      {
         if (draft.Info.Language == Language.Farsi && string.IsNullOrEmpty(draft.Info.EnglishUrl))
            _result.Add(new Problem(nameof(draft.Info.EnglishUrl), "EnglishUrl is required for Farsi posts"));
      }

      private void ValidateTitle(Draft draft)
      {
         if (_context.Infos.Any(x => x.Id != draft.Id && string.Equals(x.Title, draft.Info.Title, StringComparison.OrdinalIgnoreCase)))
            _result.Add(new Problem(nameof(draft.Info.Title), "This title already exists in the database"));
      }

      private void ValidateCodeBlocks(Draft draft)
      {
         var doc = new HtmlDocument();
         doc.LoadHtml(draft.Content);
         var num = 0;
         doc.DocumentNode.ForEachChild(node =>
         {
            var plain = node.InnerHtml.Trim();
            if (node.Is("pre.code") && !string.IsNullOrWhiteSpace(plain))
            {
               num++;

               if (!node.InnerHtml.Contains(Environment.NewLine))
               {
                  _result.Add(new Problem(nameof(Draft.Content), $"Language is not specified for the code block #{num}"));
                  return;
               }

               var lang = Draft.GetLanguage(plain);
               if (!_languages.Contains(lang))
                  _result.Add(new Problem(nameof(Draft.Content), $"Specified language in code block #{num} is not valid ({lang}...)"));
            }
         });
      }
   }
}
