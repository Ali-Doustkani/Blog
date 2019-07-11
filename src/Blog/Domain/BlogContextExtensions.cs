using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Blog.Domain
{
    public static class ContextExtensions
    {
        public static void AddOrUpdate<T>(this BlogContext context, T entity)
                   where T : DomainEntity
        {
            if (context.Set<T>().Any(x => x.Id == entity.Id))
                context.Update(entity);
            else
                context.Set<T>().Add(entity);
        }

        public static void Delete<T>(this DbSet<T> dbset, int id)
            where T : DomainEntity, new()
        {
            var entity = new T { Id = id };
            dbset.Attach(entity).State = EntityState.Deleted;
        }

        public static bool Any<T>(this DbSet<T> dbset, int id)
            where T : DomainEntity =>
            dbset.Any(x => x.Id == id);
    }
}
