using Blog.Domain;
using Blog.Domain.Blogging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Blog.Utils
{
   public class ImageContext
   {
      public ImageContext(IFileSystem fs)
      {
         _fs = fs;
      }

      private readonly IFileSystem _fs;
      private ImageCollection _images;
      private string _deleteDirectory;

      public void AddOrUpdate(ImageCollection images) =>
         _images = Assert.Arg.NotNull(images);

      public void Delete(string deleteDirectory) =>
         _deleteDirectory = deleteDirectory;

      public void SaveChanges()
      {
         if (_images != null)
         {
            RenameDirectory(_images.OldDirectory, _images.NewDirectory);
            var dir = GetDirectory(_images.NewDirectory);
            CreteDirectory(dir, _images.Images);
            WriteImages(dir, _images.Images);
            DeleteOrphanFiles(dir, _images.Images);
            _images = null;
         }
         else if (_deleteDirectory != null)
         {
            var directory = GetDirectory(_deleteDirectory);
            if (_fs.DirectoryExists(directory))
               _fs.DeleteDirectory(directory);
            _deleteDirectory = null;
         }
      }

      private void CreteDirectory(string dir, IEnumerable<Image> images)
      {
         if (images.Any() && !_fs.DirectoryExists(dir))
            _fs.CreateDirectory(dir);
      }

      private void WriteImages(string dir, IEnumerable<Image> images)
      {
         foreach (var image in images)
         {
            if (!image.IsFile)
               _fs.WriteAllBytes(Path.Combine(dir, image.Filename), image.Data);
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
