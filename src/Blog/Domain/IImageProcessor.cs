namespace Blog.Domain
{
   public interface IImageProcessor
   {
      /// <summary>
      /// Creates a small version of the original image and return a DataURL
      /// </summary>
      string Minimize(string originalImage);
   }
}
