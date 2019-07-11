using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PostInfo>(info =>
            {
                info.HasOne<Draft>()
                .WithOne(x => x.Info)
                .HasForeignKey<Draft>(x => x.Id);

                info.HasOne<Post>()
               .WithOne(x => x.Info)
               .HasForeignKey<Post>(x => x.Id);

                info.HasAlternateKey(x => x.Title);

                info.Property(x => x.Language)
                .HasConversion(new EnumToNumberConverter<Language, int>())
                .IsRequired()
                .HasDefaultValue(Language.English);

                info.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(150);

                info.Property(x => x.Summary)
                .IsRequired();

                info.Property(x => x.Tags)
                .IsRequired()
                .HasDefaultValue(string.Empty);
            });

            modelBuilder.Entity<Draft>(draft =>
            {
                draft.HasOne(x => x.Info)
                .WithOne()
                .HasForeignKey<PostInfo>(x => x.Id);
            });

            modelBuilder.Entity<Post>(post =>
            {
                post.HasOne(x => x.Info)
               .WithOne()
               .HasForeignKey<Post>(x => x.Id);

                post.HasOne(x => x.PostContent)
               .WithOne()
               .HasForeignKey<PostContent>(x => x.Id);

                post.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(200);
            });

            modelBuilder.Entity<PostContent>(pc =>
            {
                pc.ToTable("Posts");

                pc.Property(x => x.Content)
                .IsRequired();
            });
        }
    }
}
