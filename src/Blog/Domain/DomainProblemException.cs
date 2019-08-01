using System;

namespace Blog.Domain
{
   /// <summary>
   /// The deafult exception for failures of domain. 
   /// Service objects should listen to this exception and reflect the appropriate result.
   /// </summary>
   public class DomainProblemException : Exception
   {
      public DomainProblemException(string property, string message)
         : base(message)
      {
         Problem = new Problem(property, message);
      }

      public Problem Problem { get; }
   }
}
