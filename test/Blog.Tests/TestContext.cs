using Blog.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Xunit.Abstractions;

namespace Blog.Tests
{
   public class TestContext
   {
      public TestContext(ITestOutputHelper output)
      {
         _loggerFactory = new LoggerFactory();
         _loggerFactory.AddProvider(new TestOutputLoggerProvider(output));
         _connection = new SqliteConnection("DataSource=:memory:");
         _connection.Open();
         using (var ctx = GetDb())
            ctx.Database.EnsureCreated();
      }

      private readonly SqliteConnection _connection;
      private readonly ILoggerFactory _loggerFactory;

      public BlogContext GetDb()
      {
         var optionBuilder = new DbContextOptionsBuilder();
         optionBuilder.UseSqlite(_connection);
         optionBuilder.UseLoggerFactory(_loggerFactory);
         optionBuilder.EnableSensitiveDataLogging();
         return new BlogContext(optionBuilder.Options);
      }
   }

   public class TestOutputLoggerProvider : ILoggerProvider
   {
      public TestOutputLoggerProvider(ITestOutputHelper output)
      {
         _output = output;
      }

      private readonly ITestOutputHelper _output;

      public ILogger CreateLogger(string categoryName)
      {
         return new TestOutputLogger(_output);
      }

      public void Dispose() { }
   }

   public class TestOutputLogger : ILogger
   {
      public TestOutputLogger(ITestOutputHelper output)
      {
         _output = output;
      }

      private readonly ITestOutputHelper _output;
      private const int COMMAND_EXECUTING = 20100;

      public IDisposable BeginScope<TState>(TState state)
      {
         return null;
      }

      public bool IsEnabled(LogLevel logLevel) => true;

      public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
      {
         if (eventId.Id != COMMAND_EXECUTING) return;

         var log = formatter(state, exception);

         if (log.Contains("PRAGMA")) return;
         if (log.Contains("CREATE TABLE")) return;
         if (log.Contains("CREATE INDEX")) return;
         if (log.Contains("CREATE UNIQUE INDEX")) return;
         if (log.Contains("sqlite_master")) return;

         _output.WriteLine(log);
         _output.WriteLine(string.Empty);
      }
   }
}
