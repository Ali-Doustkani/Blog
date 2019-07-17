using Blog.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   public class DraftValidator
   {
      public DraftValidator(BlogContext context)
      {
         _context = Its.NotEmpty(context, nameof(context));
      }

      private readonly BlogContext _context;

      public IEnumerable<Problem> Validate(Draft draft)
      {
         var result = new List<Problem>();

         if (draft.Info.Language == Language.Farsi && string.IsNullOrEmpty(draft.Info.EnglishUrl))
            result.Add(new Problem(nameof(draft.Info.EnglishUrl), "EnglishUrl must have value for Farsi posts"));

         if (_context.Infos.Any(x => x.Id != draft.Id && string.Equals(x.Title, draft.Info.Title, StringComparison.OrdinalIgnoreCase)))
            result.Add(new Problem(nameof(draft.Info.Title), "This title already exists in the database."));

         return result;
      }
   }
}
