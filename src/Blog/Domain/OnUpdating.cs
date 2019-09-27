namespace Blog.Domain
{
   public enum UpdatingType
   {
      Removing,
      Added
   }

   public delegate void OnUpdating(UpdatingType type, DomainEntity entity);
}
