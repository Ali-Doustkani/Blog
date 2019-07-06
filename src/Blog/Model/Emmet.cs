using System.Linq;

namespace Blog.Model
{
    public static class Emmet
    {
        public static string El(string selector, string value) => selector
            .Split('>')
            .Reverse()
            .Aggregate(value, Surround);


        static string Surround(string value, string selector)
        {
            var items = selector.Split('.');
            if (items.Count() == 1)
                return $"<{selector}>{value}</{selector}>";
            return $"<{items[0]} class=\"{items[1]}\">{value}</{items[0]}>";
        }
    }
}
