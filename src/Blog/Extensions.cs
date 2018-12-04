using Microsoft.AspNetCore.Mvc;

namespace Blog
{
    public static class Extensions
    {
        public static string NameOf<T>( )
        {
            return typeof(T).Name.Replace("Controller", string.Empty);
        }
    }
}
