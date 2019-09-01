using System;

namespace Blog.Domain
{
   /// <summary>
   /// If anything happens in services that the domain is dependent upon, this exception will raise
   /// </summary>
   public class ServiceDependencyException : Exception
   {
      public ServiceDependencyException(string message, Exception innerException)
         : base(message, innerException)
      { }
   }
}
