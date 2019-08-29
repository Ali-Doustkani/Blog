using Blog.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Blog.Tests
{
   public class MockFileSystem : IFileSystem
   {
      public MockFileSystem()
      {
         log = new List<string>();
         _directories = new List<string>();
         _files = new List<string>();
      }

      public readonly List<string> log;
      private readonly List<string> _directories;
      private List<string> _files;

      public void CreateDirectory(string path)
      {
         _directories.Add(path.Standard());
         log.Add(string.Join(" ", "create-dir", path.Standard()));
      }

      public void DeleteDirectory(string path)
      {
         _directories.Remove(path.Standard());
         _files.RemoveAll(x => x.StartsWith(path.Standard()));
         log.Add(string.Join(" ", "del-dir", path.Standard()));
      }

      public void DeleteFile(string path)
      {
         _files.Remove(path.Standard());
         log.Add(string.Join(" ", "del-file", path.Standard()));
      }

      public bool DirectoryExists(string path) =>
         _directories.Contains(path.Standard());

      public string[] GetFiles(string path)
      {
         if (!DirectoryExists(path))
            throw new DirectoryNotFoundException();

         return _files.Where(x => x.StartsWith(path.Standard())).ToArray();
      }

      public void RenameDirectory(string oldDir, string newDir)
      {
         if (!_directories.Contains(oldDir.Standard()))
            throw new IOException();

         _directories.Remove(oldDir.Standard());
         _directories.Add(newDir.Standard());
         _files = _files
            .Select(x => x.StartsWith(oldDir.Standard()) ? Path.Combine(newDir.Standard(), Path.GetFileName(x)).Standard() : x)
            .ToList();
         log.Add(string.Join(" ", "rename-dir", oldDir.Standard(), newDir.Standard()));
      }

      public void WriteAllBytes(string path, byte[] data)
      {
         _files.Add(path.Standard());
         log.Add(string.Join(" ", "write-file", path.Standard(), string.Join(",", data)));
      }

      public void WriteFile(string path) =>
         WriteAllBytes(path, new byte[] { });
   }
}
