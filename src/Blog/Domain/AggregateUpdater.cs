using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   /// <summary>
   /// Implements the logic of updating aggregate lists from aggregate roots using command objects.
   /// </summary>
   /// <typeparam name="TEntry">Command Entry Object</typeparam>
   /// <typeparam name="TDomainEntity">Domain Entity Object</typeparam>
   public class AggregateUpdater<TEntry, TDomainEntity>
      where TEntry : DomainObjectEntry
   {
      public AggregateUpdater(AggregateList<TDomainEntity> aggregates, IEnumerable<TEntry> list)
      {
         _aggregates = Assert.NotNull(aggregates);
         _list = list;
      }

      private readonly AggregateList<TDomainEntity> _aggregates;
      private readonly IEnumerable<TEntry> _list;
      private Action<int, TEntry> _update;
      private Action<TEntry> _add;

      public AggregateUpdater<TEntry, TDomainEntity> OnUpdate(Action<int, TEntry> action)
      {
         _update = action;
         return this;
      }

      public void OnAdd(Action<TEntry> action)
      {
         _add = action;
         Run();
      }

      private void Run()
      {
         if (_update == null)
            throw new InvalidOperationException("Cannot update the target aggregate without Update funcationality");

         if (_add == null)
            throw new InvalidOperationException("Cannot update the target aggreagte without Add functionality");

         var list = _list ?? Enumerable.Empty<TEntry>();

         _aggregates.RemoveNotIn(list);
         foreach (var item in list)
         {
            if (int.TryParse(item.Id, out int id))
               _update(id, item);
            else
               _add(item);
         }
      }
   }
}
