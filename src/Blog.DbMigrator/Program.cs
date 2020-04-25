using Blog.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Blog.DbMigrator
{
   class Program
   {
      static async Task Main(string[] args)
      {
         if (args.Length == 1 && args[0].ToLower()=="localdb")
         {
             Console.WriteLine("Migrating LocalDb started");
             await Migrator.MigrateLocalDb();
             Console.WriteLine("LocalDb Migrated");
             return;
         }

         string server, db, username, psw;
         if (args.Length == 4)
         {
            server = args[0];
            db = args[1];
            username = args[2];
            psw = args[3];
         }
         else
         {
            Console.WriteLine("Server:");
            server = Console.ReadLine();

            Console.WriteLine("Database Name:");
            db = Console.ReadLine();

            Console.WriteLine("Username:");
            username = Console.ReadLine();

            Console.WriteLine("Password:");
            psw = Console.ReadLine();
         }

         try
         {
            await Migrator.Migrate(server, db, username, psw);
            Console.WriteLine("Done!");
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex.Message);
         }
      }
   }
}
