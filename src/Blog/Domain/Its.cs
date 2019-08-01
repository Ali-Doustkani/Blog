using System;

namespace Blog.Domain
{
   public class Its
   {
      public static T NotEmpty<T>(T parameter, string name)
      {
         if (typeof(T) == typeof(string) && string.IsNullOrEmpty(Convert.ToString(parameter)))
            throw new DomainProblemException(name, "Value is required");

         if (parameter == null)
            throw new DomainProblemException(name, "Value is required");

         return parameter;
      }
   }
}
