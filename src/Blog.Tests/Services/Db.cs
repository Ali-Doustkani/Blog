using Blog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Tests.Services
{
    public static class Db
    {
        public static BlogContext CreateInMemory()
        {
            var optionBuilder = new DbContextOptionsBuilder();
            optionBuilder.UseInMemoryDatabase("BlogInMemoryDb");

            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            optionBuilder.UseInternalServiceProvider(serviceProvider);

            var ctx = new BlogContext(optionBuilder.Options);
            ctx.Database.EnsureCreated();
            return ctx;
        }
    }
}
