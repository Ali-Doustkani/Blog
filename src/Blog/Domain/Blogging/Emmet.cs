using System.Linq;

namespace Blog.Domain.Blogging
{
   public static class Emmet
   {
      public static string El(string selector, object value) => selector
          .Split('>')
          .Reverse()
          .Aggregate(value.ToString(), Surround);


      static string Surround(string value, string selector)
      {
         var items = selector.Split('.');
         if (items.Count() == 1)
            return $"<{selector}>{value}</{selector}>";
         return $"<{items[0]} class=\"{items[1]}\">{value}</{items[0]}>";
      }
   }
}
