using System.IO;

namespace Blog.Domain
{
    public class Image
    {
        public Image(string fullname, byte[] data)
        {
            Fullname = fullname;
            Data = data;
        }

        public string Filename { get => Path.GetFileName(Fullname); }
        public string Fullname { get; }
        public byte[] Data { get; }
    }
}
