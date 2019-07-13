using Blog.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests.Services
{
    public static class Db
    {
        public static BlogContext CreateInMemory()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var optionBuilder = new DbContextOptionsBuilder();
            optionBuilder.UseSqlite(connection);

            using (var ctx = new BlogContext(optionBuilder.Options))
                ctx.Database.EnsureCreated();

            return new BlogContext(optionBuilder.Options);
        }

        public static DbContextOptions CreateOptions()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var optionBuilder = new DbContextOptionsBuilder();
            optionBuilder.UseSqlite(connection);
            return optionBuilder.Options;
        }
    }
}
