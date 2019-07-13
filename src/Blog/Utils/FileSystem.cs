using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace Blog.Utils
{
   public interface IFileSystem
   {
      void CreateDirectory(string path);
      bool DirectoryExists(string path);
      void DeleteDirectory(string path);
      void RenameDirectory(string oldDir, string newDir);
      string[] GetFiles(string path);
      void DeleteFile(string path);
      void WriteAllBytes(string path, byte[] data);
   }

   public class FileSystem : IFileSystem
   {
      public FileSystem(ILogger<FileSystem> logger)
      {
         _logger = logger;
      }

      private readonly ILogger _logger;

      public void CreateDirectory(string path)
      {
         if (!Directory.CreateDirectory(path).GetFiles().Any())
            _logger.LogInformation("Create Directory: {0}", path);
      }

      public void DeleteDirectory(string path)
      {
         Directory.Delete(path, true);
         _logger.LogInformation("Delete Directory: {0}", path);
      }

      public void DeleteFile(string path)
      {
         File.Delete(path);
         _logger.LogInformation("Delete File: {0}", path);
      }

      public bool DirectoryExists(string path) =>
          Directory.Exists(path);

      public string[] GetFiles(string path) =>
          Directory.GetFiles(path);

      public void WriteAllBytes(string path, byte[] data)
      {
         File.WriteAllBytes(path, data);
         _logger.LogInformation("Write File: {0}, Size: {1}", path, data.Length);
      }

      public void RenameDirectory(string oldDir, string newDir)
      {
         Directory.Move(oldDir, newDir);
         _logger.LogInformation("Rename Directory: {0} ==> {1}", oldDir, newDir);
      }
   }
}
