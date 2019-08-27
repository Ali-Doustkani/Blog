using System;

namespace Blog.Domain
{
   public static class Assert
   {
      static Assert()
      {
         Op = new Assertions<InvalidOperationException>();
         Arg = new Assertions<ArgumentException>();
      }

      public static Assertions<InvalidOperationException> Op { get; }
      public static Assertions<ArgumentException> Arg { get; }
   }

   public class Assertions<TException>
        where TException : Exception, new()
   {
      public T NotNull<T>(T input)
      {
         if (input == null)
            throw new TException();
         return input;
      }

      public string NotNull(string input)
      {
         if (string.IsNullOrWhiteSpace(input))
            throw new TException();
         return input;
      }
   }
}
