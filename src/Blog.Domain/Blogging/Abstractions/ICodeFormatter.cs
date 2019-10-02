using System.Threading.Tasks;

namespace Blog.Domain.Blogging.Abstractions
{
   public interface ICodeFormatter
   {
      /// <summary>
      /// Tokenize the code to HTML <span>s
      /// </summary>
      /// <returns>HTML of beautified code</returns>
      /// <exception cref="ServiceDependencyException">If anything happens during formatting.</exception>
      Task<string> FormatAsync(string language, string code);
   }
}
