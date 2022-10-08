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
            eb.HasMany(x => x.LikedPosts).WithMany(x => x.Likes);
        });
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
}