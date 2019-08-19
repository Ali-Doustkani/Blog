using Blog.Domain.Blogging;
using Blog.Domain.DeveloperStory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   public class BlogContext : IdentityDbContext<IdentityUser>
   {
      public BlogContext(DbContextOptions options)
          : base(options)
      { }

      public DbSet<PostInfo> Infos { get; set; }
      public DbSet<Draft> Drafts { get; set; }
      public DbSet<Post> Posts { get; set; }
      public DbSet<PostContent> PostContents { get; set; }
      public DbSet<Developer> Developers { get; set; }

      public void AddOrUpdate<T>(T entity)
         where T : DomainEntity
      {
         if (Set<T>().Any(x => x.Id == entity.Id))
            Update(entity);
         else
            Set<T>().Add(entity);
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         modelBuilder.Entity<PostInfo>(info =>
         {
            info
               .HasOne<Draft>()
               .WithOne(x => x.Info)
               .HasForeignKey<Draft>(x => x.Id);

            info
               .HasOne<Post>()
              .WithOne(x => x.Info)
              .HasForeignKey<Post>(x => x.Id);

            info
               .HasIndex(x => x.Title)
               .IsUnique();

            info
               .Property(x => x.Language)
               .HasConversion(new EnumToNumberConverter<Language, int>())
               .IsRequired()
               .HasDefaultValue(Language.English);

            info
               .Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(150);

            info
            .HasIndex(x => x.EnglishUrl)
            .IsUnique();

            info
               .Property(x => x.Summary)
               .IsRequired();

            info
               .Property(x => x.Tags)
               .IsRequired();
         });

         modelBuilder.Entity<Draft>(draft =>
         {
            draft
               .HasOne(x => x.Info)
               .WithOne()
               .HasForeignKey<PostInfo>(x => x.Id);
         });

         modelBuilder.Entity<Post>(post =>
         {
            post
               .HasOne(x => x.Info)
              .WithOne()
              .HasForeignKey<Post>(x => x.Id);

            post
               .HasOne(x => x.PostContent)
              .WithOne()
              .HasForeignKey<PostContent>(x => x.Id);

            post
               .Property(x => x.Url)
               .IsRequired()
               .HasMaxLength(200);
         });

         modelBuilder.Entity<PostContent>(pc =>
         {
            pc.ToTable("Posts");

            pc
               .Property(x => x.Content)
               .IsRequired();
         });

         modelBuilder.Entity<Developer>(dev =>
         {
            dev
            .Property(x => x.Summary)
            .IsRequired();

            dev
            .Property(x => x.Skills)
            .IsRequired();

            dev
            .Metadata
            .FindNavigation("Experiences")
            .SetPropertyAccessMode(PropertyAccessMode.Field);

            dev
            .Metadata
            .FindNavigation("SideProjects")
            .SetPropertyAccessMode(PropertyAccessMode.Field);
         });

         modelBuilder.Entity<Experience>(ex =>
         {
            ex.ToTable("Experiences");

            ex
            .Property(x => x.Company)
            .IsRequired();

            ex
            .Property(x => x.Content)
            .IsRequired();

            ex
            .Property("DeveloperId")
            .IsRequired();
         });

         modelBuilder.Entity<SideProject>(sp =>
         {
            sp.ToTable("SideProjects");

            sp
            .Property(x => x.Title)
            .IsRequired();

            sp
            .Property(x => x.Content)
            .IsRequired();

            sp
            .Property("DeveloperId")
            .IsRequired();
         });
      }
   }
}
