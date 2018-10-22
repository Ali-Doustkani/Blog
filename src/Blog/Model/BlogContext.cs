using Microsoft.EntityFrameworkCore;

namespace Blog.Model
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(post =>
            {
                post.Property(x => x.Title).IsRequired().HasMaxLength(150);
                post.Property(x => x.Summary).IsRequired();
                post.Property(x => x.Content).IsRequired();
                post.Property(x => x.Tags).IsRequired().HasDefaultValue(string.Empty);
            });
        }
    }
}
