using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure
{
    public static class Migrator
    {
        public static async Task Migrate(string server, string database, string username, string password)
        {
            await Migrate($"Data Source={server};Initial Catalog={database};User ID={username};Password={password};Connect Timeout=60;");
        }

        public static async Task MigrateLocalDb()
        {
            await Migrate("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BlogDb;Integrated Security=True;");
        }

        private static async Task Migrate(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(connectionString);
            var context = new BlogContext(optionsBuilder.Options);
            context.Database.Migrate();

#if DEBUG
            Console.WriteLine("Creating Test User");
            var testUser = new IdentityUser("test");
            var store = new UserOnlyStore<IdentityUser, BlogContext>(context);
            var passwordHasher = new PasswordHasher<IdentityUser>();
            var userValidators = Enumerable.Empty<IUserValidator<IdentityUser>>();
            var passwordValidators = Enumerable.Empty<IPasswordValidator<IdentityUser>>();
            var normalizer = new UpperInvariantLookupNormalizer();
            var errDescriber = new IdentityErrorDescriber();
            var userManager = new UserManager<IdentityUser>(store, null, passwordHasher, userValidators, passwordValidators, normalizer, errDescriber, null, null);
            await userManager.CreateAsync(testUser, "123");
#endif
        }
    }
}
