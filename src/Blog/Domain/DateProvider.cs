using System;

namespace Blog.Domain
{
   public interface IDateProvider
   {
      DateTime Now { get; }
   }

   public class DefaultDateProvider : IDateProvider
   {
      public DateTime Now => DateTime.Now;
   }
}
