using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Blog.Utils
{
   public static class DependencyScanner
   {
      public static IServiceCollection AddBlogTypes(this IServiceCollection services)
      {
         var types = Assembly
            .GetExecutingAssembly()
            .GetExportedTypes()
            .Where(x => x.IsClass && !x.IsAbstract && !x.IsGenericType && !x.IsNested);

         foreach (var type in types)
         {
            var interfaces = type.GetTypeInfo().ImplementedInterfaces.Where(x => x != typeof(IDisposable) && x.IsPublic);
            foreach (var intr in interfaces)
               services.Add(new ServiceDescriptor(intr, type, ServiceLifetime.Transient));

            if (!interfaces.Any() && EndsWithConvention(type.Name))
            {
               services.Add(new ServiceDescriptor(type, type, ServiceLifetime.Transient));
            }
         }

         return services;
      }

      private static bool EndsWithConvention(string typename)
      {
         var endings = new[] { "Command", "Validator" };
         foreach (var ending in endings)
            if (typename.EndsWith(ending))
               return true;
         return false;
      }
   }
}
