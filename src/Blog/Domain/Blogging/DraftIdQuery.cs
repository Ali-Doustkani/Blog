using Blog.Infra;
using System;
using System.Linq.Expressions;

namespace Blog.Domain.Blogging
{
   public class DraftIdQuery : AbstractQuery<Draft>
   {
      public DraftIdQuery(int id)
      {
         _id = id;
      }

      private int _id;

      public override Expression<Func<Draft, bool>> Query() =>
           x => x.Id == _id;
   }
}
