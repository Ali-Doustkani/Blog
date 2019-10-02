using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Blog.Utils
{
   public static class Extensions
   {
      public static void AddModelErrors(this ModelStateDictionary dic, IEnumerable<string> problems)
      {
         foreach (var prob in problems)
            dic.AddModelError(string.Empty, prob);
      }
   }
}
