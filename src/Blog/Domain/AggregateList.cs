using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   public class AggregateList<T> : List<T>
   {
      public AggregateList(Func<T, int> getId)
      {
         _getId = getId;
      }

      public AggregateList(Func<T, int> getId, IEnumerable<T> collection)
         : base(collection)
      {
         _getId = getId;
      }

      private Func<T, int> _getId;

      public void RemoveNotIn(IEnumerable<DomainObjectEntry> entries)
      {
         RemoveAll(x => !entries.Select(y => y.Id).Contains(_getId(x).ToString()));
      }

      public T Single(string id)
      {
         return Enumerable.Single(this, x => _getId(x).ToString() == id);
      }
   }
}
