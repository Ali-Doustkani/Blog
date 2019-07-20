﻿using Blog.Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Blog.Utils
{
   public interface IImageContext
   {
      void SaveChanges(string oldPostDirectory, string postDirectory, IEnumerable<Image> images);
      void Delete(string urlTitle);
   }

   public class ImageContext : IImageContext
   {
      public ImageContext(IFileSystem fs)
      {
         _fs = fs;
      }

      private readonly IFileSystem _fs;

      public void SaveChanges(string oldPostDirectory, string postDirectory, IEnumerable<Image> images)
      {
         RenameDirectory(oldPostDirectory, postDirectory);
         var dir = GetDirectory(postDirectory);
         CreteDirectory(dir, images);
         WriteImages(dir, images);
         DeleteOrphanFiles(dir, images);
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

      public void Delete(string postSlug)
      {
         var directory = GetDirectory(postSlug);
         if (_fs.DirectoryExists(directory))
            _fs.DeleteDirectory(directory);
      }

      private string GetDirectory(string postDirectory) =>
          Path.Combine("wwwroot", "images", "posts", postDirectory);
   }
}
