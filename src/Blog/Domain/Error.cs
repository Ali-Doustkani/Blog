namespace Blog.Domain
{
   public class Error
   {
      public Error(string message)
      {
         Message = Assert.NotNull(message);
      }

      public Error(string property, string message)
      {
         Property = property;
         Message = Assert.NotNull(message);
      }

      /// <summary>
      /// The proprerty that has the probelm. 
      /// Empty string refers to the object itself.
      /// </summary>
      public string Property { get; }

      public string Message { get; }
   }
}
