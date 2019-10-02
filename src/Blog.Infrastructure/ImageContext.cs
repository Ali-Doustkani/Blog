using Blog.Domain;
using Blog.Domain.Blogging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Infrastructure
{
   public class ImageContext
   {
      public ImageContext(IFileSystem fs)
      {
         _fs = fs;
      }

      private readonly IFileSystem _fs;

      public async Task AddOrUpdateAsync(ImageCollection images)
      {
         Assert.NotNull(images);
         RenameDirectory(images.OldDirectory, images.NewDirectory);
         var dir = GetDirectory(images.NewDirectory);
         CreteDirectory(dir, images.Images);
         await WriteImages(dir, images.Images);
         DeleteOrphanFiles(dir, images.Images);
      }

      public void Delete(string deleteDirectory)
      {
         var directory = GetDirectory(deleteDirectory);
         if (_fs.DirectoryExists(directory))
            _fs.DeleteDirectory(directory);
      }

      private void CreteDirectory(string dir, IEnumerable<Image> images)
      {
         if (images.Any() && !_fs.DirectoryExists(dir))
            _fs.CreateDirectory(dir);
      }

      private async Task WriteImages(string dir, IEnumerable<Image> images)
      {
         foreach (var image in images)
         {
            if (!image.IsFile)
               await _fs.WriteAllBytesAsync(Path.Combine(dir, image.Filename), image.Data);
         }
      }

      private void DeleteOrphanFiles(string dir, IEnumerable<Image> images)
      {
         if (!_fs.DirectoryExists(dir)) return;

         foreach (var file in _fs.GetFiles(dir))
         {
            if (!images.Any(x => x.Filename == Path.GetFileName(file)))
               _fs.DeleteFile(file);
         }

         if (!_fs.GetFiles(dir).Any())
            _fs.DeleteDirectory(dir);
      }

      private void RenameDirectory(string oldPostDirectory, string postDirectory)
      {
         if (string.IsNullOrEmpty(oldPostDirectory) || oldPostDirectory == postDirectory)
            return;

         if (!_fs.DirectoryExists(GetDirectory(oldPostDirectory)))
            return;

         _fs.RenameDirectory(GetDirectory(oldPostDirectory), GetDirectory(postDirectory));
      }

      private string GetDirectory(string postDirectory) =>
          Path.Combine("wwwroot", "images", "posts", postDirectory);
   }
}
