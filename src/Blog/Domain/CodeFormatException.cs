using System;

namespace Blog.Domain
{
   public class CodeFormatException : Exception
   {
      public CodeFormatException(Exception innerException)
         : base("Formatting code failed", innerException)
      { }
   }
}
