namespace Blog.Domain
{
   public interface IStorageState
   {
      void Modify(params object[] entity);
      void Detach(params object[] entity);
   }
}
