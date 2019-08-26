using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   public class AggregateList<T> : List<T>
      where T : DomainEntity
   {
      public AggregateList()
      { }

      public AggregateList(IEnumerable<T> collection)
         : base(collection)
      { }

      public void RemoveNotIn(IEnumerable<DomainObjectEntry> entries)
      {
         RemoveAll(x => !entries.Select(y => y.Id).Contains(x.Id.ToString()));
      }

      public T Single(string id)
      {
         return Enumerable.Single(this, x => x.Id.ToString() == id);
      }
   }
}
