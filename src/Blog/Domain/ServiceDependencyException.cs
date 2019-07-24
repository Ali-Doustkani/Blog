using System;

namespace Blog.Domain
{
   public class ServiceDependencyException : Exception
   {
      public ServiceDependencyException(string message, Exception innerException)
         : base(message, innerException)
      { }
   }
}
