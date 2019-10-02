namespace Blog.Domain
{
   /// <summary>
   /// The base class for all change objects. 
   /// Change objects are passed to domain entities to run commands.
   /// </summary>
   public class DomainObjectEntry
   {
      public string Id { get; set; }
   }
}
