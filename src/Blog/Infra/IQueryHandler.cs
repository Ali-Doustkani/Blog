namespace Blog.Infra
{
   public interface IQueryHandler
   {
      T Execute<T>(AbstractQuery<T> query)
         where T : class;

      bool Any<T>(AbstractQuery<T> query)
         where T : class;
   }
}
