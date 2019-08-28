using Blog.Infra;
using System.Linq;

namespace Blog.Storage
{
   public class QueryHandler : IQueryHandler
   {
      public QueryHandler(BlogContext context)
      {
         _context = context;
      }

      private readonly BlogContext _context;

      public bool Any<T>(AbstractQuery<T> query)
         where T : class =>
         _context.Set<T>().Any(query.Query());

      public T Execute<T>(AbstractQuery<T> query)
         where T : class =>
         _context.Set<T>().SingleOrDefault(query.Query());
   }
}
