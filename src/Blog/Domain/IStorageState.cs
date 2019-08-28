namespace Blog.Domain
{
   public interface IStorageState
   {
      void Add(params object[] entities);
      void Modify(params object[] entities);
      void Delete(params object[] entities);
      void Detach(params object[] entities);
   }
}
