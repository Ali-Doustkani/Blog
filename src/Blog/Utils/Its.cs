using System;

namespace Blog.Utils
{
   public class Its
   {
      public static T NotEmpty<T>(T parameter, string name = null)
      {
         if (typeof(T) == typeof(string) && string.IsNullOrEmpty(Convert.ToString(parameter)))
            throw new ArgumentNullException(name);

         if (parameter == null)
            throw new ArgumentNullException(name);

         return parameter;
      }
   }
}
