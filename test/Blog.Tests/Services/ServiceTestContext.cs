using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Storage;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Tests.Services
{
   public class ServiceTestContext<TService>
      where TService : IDisposable
   {
      public ServiceTestContext()
      {
         var connection = new SqliteConnection("DataSource=:memory:");
         connection.Open();
         var optionBuilder = new DbContextOptionsBuilder();
         optionBuilder.UseSqlite(connection);
         _options = optionBuilder.Options;
         using (var ctx = new BlogContext(_options))
            ctx.Database.EnsureCreated();

         _types = new List<Type>();
         _mocks = new Dictionary<Type, object>();
      }

      private readonly DbContextOptions _options;
      private readonly List<Type> _types;
      private readonly Dictionary<Type, object> _mocks;
      private BlogContext _currentContext;

      public void WithType<T>() =>
         _types.Add(typeof(T));

      public void WithMock<T>()
         where T : class =>
         _mocks.Add(typeof(T), new Mock<T>());

      public Mock<T> GetMock<T>()
         where T : class =>
         (Mock<T>)_mocks[typeof(T)];

      public BlogContext GetDatabase()
      {
         _currentContext = new BlogContext(_options);
         return _currentContext;
      }

      public void Seed(Action<BlogContext> seedAction)
      {
         using (var db = GetDatabase())
         {
            db.Database.EnsureCreated();
            seedAction(db);
            db.SaveChanges();
         }
      }

      public TService GetService()
      {
         var args = new List<object>();
         foreach (var param in typeof(TService).GetConstructors().Single().GetParameters())
            args.Add(CreateInstance(param.ParameterType));
         return (TService)Activator.CreateInstance(typeof(TService), args.ToArray());
      }

      private object CreateInstance(Type type)
      {
         if (type == typeof(BlogContext))
            return GetDatabase();

         if (type == typeof(IMapper))
            return CreateMapper();

         if (type == typeof(IStorageState))
            return new Storage.StorageState(_currentContext);

         if (type == typeof(IHtmlProcessor))
            return new HtmlProcessor(GetMock<ICodeFormatter>().Object, GetMock<IImageProcessor>().Object);

         if (type == typeof(IDateProvider))
            return new DefaultDateProvider();

         if (_mocks.ContainsKey(type))
            return ((Mock)_mocks[type]).Object;

         var dep = _types.SingleOrDefault(x => x == type);
         var ctor = dep.GetConstructors().Single();
         if (!ctor.GetParameters().Any())
            return Activator.CreateInstance(dep);

         var parameters = new List<object>();
         foreach (var ctorParam in ctor.GetParameters())
            parameters.Add(CreateInstance(ctorParam.ParameterType));

         return Activator.CreateInstance(type, parameters.ToArray());
      }

      private IMapper CreateMapper() =>
         new MapperConfiguration(cfg =>
         {
            var profiles = typeof(TService).Assembly
            .GetExportedTypes()
            .Where(x => x.IsSubclassOf(typeof(Profile)));

            foreach (var profile in profiles)
               cfg.AddProfile(profile);
         })
         .CreateMapper();
   }
}
