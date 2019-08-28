using Blog.Infra;
using System;
using System.Linq.Expressions;

namespace Blog.Domain.Blogging
{
   public class DraftTitleQuery : AbstractQuery<Draft>
   {
      public DraftTitleQuery(int id, string title)
      {
         _id = id;
         _title = title;
      }

      private int _id;
      private string _title;

      public override Expression<Func<Draft, bool>> Query() =>
         x => x.Id != _id && string.Equals(x.Title, _title, StringComparison.OrdinalIgnoreCase);
   }
}
