namespace Blog.Domain
{
   /// <summary>
   /// The base class for all domain entities.
   /// </summary>
   public abstract class DomainEntity
   {
      protected DomainEntity(int id)
      {
         Id = id;
      }

      public int Id { get; private set; }
   }
}
