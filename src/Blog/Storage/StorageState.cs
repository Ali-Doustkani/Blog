using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Storage
{
   public class StorageState : IStorageState
   {
      public StorageState(BlogContext context)
      {
         _context = context;
      }

      private readonly BlogContext _context;

      public void Detach(params object[] entities)
      {
         foreach (var entry in entities)
            _context.Entry(entry).State = EntityState.Detached;
      }

      public void Modify(params object[] entities)
      {
         foreach (var entry in entities)
            _context.Entry(entry).State = EntityState.Modified;
      }
   }
}
