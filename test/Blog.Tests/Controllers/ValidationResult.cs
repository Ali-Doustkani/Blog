namespace Blog.Tests.Controllers
{
   public class ValidationResult
   {
      public string Title { get; set; }
      public Item[] Errors { get; set; }
   }

   public class Item
   {
      public string Error { get; set; }
      public object[] Path { get; set; }
   }
}
