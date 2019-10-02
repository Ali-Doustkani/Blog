using Blog.Domain;
using Blog.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
         _directories.Add(path.StandardPath());
         log.Add(string.Join(" ", "create-dir", path.StandardPath()));
      }

      public void DeleteDirectory(string path)
      {
         _directories.Remove(path.StandardPath());
         _files.RemoveAll(x => x.StartsWith(path.StandardPath()));
         log.Add(string.Join(" ", "del-dir", path.StandardPath()));
      }

      public void DeleteFile(string path)
      {
         _files.Remove(path.StandardPath());
         log.Add(string.Join(" ", "del-file", path.StandardPath()));
      }

      public bool DirectoryExists(string path) =>
         _directories.Contains(path.StandardPath());

      public string[] GetFiles(string path)
      {
         if (!DirectoryExists(path))
            throw new DirectoryNotFoundException();

         return _files.Where(x => x.StartsWith(path.StandardPath())).ToArray();
      }

      public void RenameDirectory(string oldDir, string newDir)
      {
         if (!_directories.Contains(oldDir.StandardPath()))
            throw new IOException();

         _directories.Remove(oldDir.StandardPath());
         _directories.Add(newDir.StandardPath());
         _files = _files
            .Select(x => x.StartsWith(oldDir.StandardPath()) ? Path.Combine(newDir.StandardPath(), Path.GetFileName(x)).StandardPath() : x)
            .ToList();
         log.Add(string.Join(" ", "rename-dir", oldDir.StandardPath(), newDir.StandardPath()));
      }

      public Task WriteAllBytesAsync(string path, byte[] data)
      {
         _files.Add(path.StandardPath());
         log.Add(string.Join(" ", "write-file", path.StandardPath(), string.Join(",", data)));
         return Task.CompletedTask;
      }

      public void WriteFile(string path) =>
         WriteAllBytesAsync(path, new byte[] { });
   }
}
