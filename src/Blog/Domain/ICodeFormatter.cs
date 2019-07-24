namespace Blog.Domain
{
   public interface ICodeFormatter
   {
      /// <summary>
      /// Tokenize the code to HTML <span>s
      /// </summary>
      /// <returns>HTML of beautified code</returns>
      /// <exception cref="ServiceDependencyException">If anything happens during formatting.</exception>
      string Format(string language, string code);
   }
}
