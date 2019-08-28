using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Utils;
using Microsoft.EntityFrameworkCore;

namespace Blog.Storage
{
   public class StorageState : IStorageState
   {
      public StorageState(BlogContext context, ImageContext imageContext)
      {
         _context = context;
         _imageContext = imageContext;
      }

      private readonly BlogContext _context;
      private readonly ImageContext _imageContext;

      public void Detach(params object[] entities)
      {
         foreach (var entry in entities)
            _context.Entry(entry).State = EntityState.Detached;
      }

      public void Add(params object[] entities)
      {
         foreach (var entry in entities)
            _context.Entry(entry).State = EntityState.Added;
      }

      public void Modify(params object[] entities)
      {
         if (entities[0] is ImageCollection images)
         {
            _imageContext.AddOrUpdate(images);
         }
         else
         {
            foreach (var entry in entities)
               _context.Entry(entry).State = EntityState.Modified;
         }
      }

      public void Delete(params object[] entities)
      {
         if (entities[0] is ImageCollection images)
            _imageContext.Delete(images.NewDirectory);
      }
   }
}
