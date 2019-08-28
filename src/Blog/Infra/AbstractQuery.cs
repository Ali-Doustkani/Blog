using System;
using System.Linq.Expressions;

namespace Blog.Infra
{
   public abstract class AbstractQuery<T>
   {
      public abstract Expression<Func<T, bool>> Query();
   }
}
