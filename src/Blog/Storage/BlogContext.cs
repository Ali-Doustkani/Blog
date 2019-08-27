using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Domain.DeveloperStory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq;

namespace Blog.Storage
{
   public class BlogContext : IdentityDbContext<IdentityUser>
   {
      public BlogContext(DbContextOptions options)
          : base(options)
      { }

      public DbSet<Draft> Drafts { get; set; }
      public DbSet<Post> Posts { get; set; }
      public DbSet<Developer> Developers { get; set; }

      public Developer GetDeveloper() =>
         Developers
         .Include(x => x.Experiences)
         .Include(x => x.SideProjects)
         .Include(x => x.Educations)
         .SingleOrDefault();

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

         modelBuilder.Entity<Draft>(draft =>
         {
            draft.Property("_publishDate").HasColumnName("PublishDate");
         });

         modelBuilder.Entity<Post>(post =>
         {
            post.Property(x => x.Language).IsRequired();
            post.Property(x => x.PublishDate).IsRequired();
            post.Property(x => x.Summary).IsRequired();
            post.Property(x => x.Tags).IsRequired();
            post.Property(x => x.Title).IsRequired();
            post.Property(x => x.Content).IsRequired();
            post
            .Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(200);
         });

         modelBuilder.Entity<Developer>(dev =>
         {
            dev
            .Property("_summary")
            .HasColumnName("Summary")
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

            dev
            .Metadata
            .FindNavigation("Educations")
            .SetPropertyAccessMode(PropertyAccessMode.Field);
         });

         modelBuilder.Entity<Experience>(ex =>
         {
            ex.ToTable("Experiences");

            ex
            .Property(x => x.Company)
            .IsRequired();

            ex
            .Property("_content")
            .HasColumnName("Content")
            .IsRequired();

            ex
            .Property("DeveloperId")
            .IsRequired();

            ex.OwnsOne(x => x.Period, p =>
            {
               p.Property(y => y.StartDate).HasColumnName("StartDate");
               p.Property(y => y.EndDate).HasColumnName("EndDate");
            });
         });

         modelBuilder.Entity<SideProject>(sp =>
         {
            sp.ToTable("SideProjects");

            sp
            .Property(x => x.Title)
            .IsRequired();

            sp
            .Property("_content")
            .HasColumnName("Content")
            .IsRequired();

            sp
            .Property("DeveloperId")
            .IsRequired();
         });

         modelBuilder.Entity<Education>(ed =>
         {
            ed
            .Property(x => x.Degree)
            .IsRequired();

            ed
            .Property(x => x.University)
            .IsRequired();

            ed
            .OwnsOne(x => x.Period, p =>
            {
               p.Property(y => y.StartDate).HasColumnName("StartDate");
               p.Property(y => y.EndDate).HasColumnName("EndDate");
            });

            ed
            .Property("DeveloperId")
            .IsRequired();
         });
      }
   }
}
