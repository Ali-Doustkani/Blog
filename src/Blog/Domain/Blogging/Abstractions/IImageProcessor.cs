using System.Threading.Tasks;

namespace Blog.Domain.Blogging.Abstractions
{
   public interface IImageProcessor
   {
      /// <summary>
      /// Creates a small version of the original image and return a DataURL
      /// </summary>
      Task<string> Minimize(string originalImage);
   }
}
