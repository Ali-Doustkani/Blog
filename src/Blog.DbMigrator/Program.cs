using Blog.Infrastructure;
using System;

namespace Blog.DbMigrator
{
   class Program
   {
      static void Main(string[] args)
      {
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
            Migrator.Migrate(server, db, username, psw);
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex.Message);
         }
      }
   }
}
