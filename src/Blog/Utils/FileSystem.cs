using System.IO;

namespace Blog.Utils
{
    public interface IFileSystem
    {
        void Write(string filename, byte[] data);
    }

    public class FileSystem : IFileSystem
    {
        public void Write(string filename, byte[] data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            File.WriteAllBytes(filename, data);
        }
    }
}
