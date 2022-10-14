using Microsoft.EntityFrameworkCore;
using PostIt.Web.Models;

namespace PostIt.Web.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) 
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(eb =>
        {
            eb.HasMany(x => x.Posts).WithOne(x => x.Author);
            eb.HasMany(x => x.PostLiked).WithMany(x => x.Likes);
        });

        modelBuilder.Entity<Post>(eb =>
        {
            eb.HasOne(x => x.Author).WithMany(x => x.Posts);
            eb.HasMany(x => x.Likes).WithMany(x => x.PostLiked);
        });
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
}