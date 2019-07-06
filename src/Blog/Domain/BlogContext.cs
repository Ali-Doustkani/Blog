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

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Post>(post =>
            {
                post.HasAlternateKey(x => x.Title);
                post.Property(x => x.Language)
                    .HasConversion(new EnumToNumberConverter<Language, int>())
                    .IsRequired()
                    .HasDefaultValue(Language.English);
                post.Property(x => x.Title).IsRequired().HasMaxLength(150);
                post.Property(x => x.UrlTitle).IsRequired().HasMaxLength(200).HasDefaultValue("[NOT SET]");
                post.Property(x => x.Summary).IsRequired();
                post.Property(x => x.MarkedContent).IsRequired();
                post.Property(x => x.DisplayContent).IsRequired();
                post.Property(x => x.Tags).IsRequired().HasDefaultValue(string.Empty);
            });
        }
    }
}
