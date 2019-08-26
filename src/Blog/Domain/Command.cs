using System;
using System.Collections.Generic;

namespace Blog.Domain
{
   public abstract class Command<TCommand>
      where TCommand : class
   {
      public AggregateUpdater<TEntry, TDomainEntity> Update<TEntry, TDomainEntity>(AggregateList<TDomainEntity> aggregates, Func<TCommand, IEnumerable<TEntry>> listFunc)
         where TEntry : DomainObjectEntry
         where TDomainEntity : DomainEntity
      {
         return new AggregateUpdater<TEntry, TDomainEntity>(aggregates, listFunc(this as TCommand));
      }
   }
}
