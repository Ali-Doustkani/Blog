using System.Collections.Generic;

namespace Blog.Domain.Blogging
{
   /// <summary>
   /// All the images of a draft content or its post.
   /// Both the Draft and the Post entities use the same image sources, which are packaged using this object.
   /// </summary>
   public class ImageCollection
   {
      public ImageCollection(IEnumerable<Image> images, string oldDirectory, string newDirectory)
      {
         Images = images;
         OldDirectory = oldDirectory;
         NewDirectory = newDirectory;
      }

      public IEnumerable<Image> Images { get; }
      public string OldDirectory { get; }
      public string NewDirectory { get; }
   }
}
