using Blog.Storage;
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
         _context = context;

      }

      private readonly BlogContext _context;
      private List<Error> _result;

      public IEnumerable<Error> Validate(Draft draft)
      {
         _result = new List<Error>();
         ValidateEnglishUrl(draft);
         ValidateTitle(draft);
         _result.AddRange(Draft.ValidateCodeBlocks(draft.Content));
         return _result;
      }

      // publish
      private void ValidateEnglishUrl(Draft draft)
      {
         if (draft.Language == Language.Farsi && string.IsNullOrEmpty(draft.EnglishUrl))
            _result.Add(new Error(nameof(draft.EnglishUrl), "EnglishUrl is required for Farsi posts"));
      }

      private void ValidateTitle(Draft draft)
      {
         if (_context.Drafts.Any(x => x.Id != draft.Id && string.Equals(x.Title, draft.Title, StringComparison.OrdinalIgnoreCase)))
            _result.Add(new Error(nameof(draft.Title), "This title already exists in the database"));
      }
   }
}
