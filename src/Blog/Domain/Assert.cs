using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Blog.Domain
{
   public static class Assert
   {
      static Assert()
      {
         Op = new OperationAssertions();
         Arg = new ArgumentAssertions();
      }

      public static OperationAssertions Op { get; }
      public static ArgumentAssertions Arg { get; }
   }

   public abstract class Assertions
   {
      public T NotNull<T>(T input, string name = null)
      {
         if (input == null)
            throw Exc(name);
         return input;
      }

      public string NotNull(string input, string name = null)
      {
         if (string.IsNullOrWhiteSpace(input))
            throw Exc(name);
         return input;
      }

      protected abstract Exception Exc(string name);
   }

   public class OperationAssertions : Assertions
   {
      protected override Exception Exc(string name) =>
         name == null
              ? new InvalidOperationException()
              : new InvalidOperationException($"{name} is required for this operation.");


   }

   public class ArgumentAssertions : Assertions
   {
      protected override Exception Exc(string name) =>
         name == null
              ? new InvalidOperationException()
              : new InvalidOperationException($"{name} is required for this operation.");
   }
}
