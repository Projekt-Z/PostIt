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
            eb.HasMany(x => x.Posts).WithOne(x => x.Author).OnDelete(DeleteBehavior.ClientCascade);
            eb.HasMany(x => x.PostLiked).WithMany(x => x.Likes);
            eb.HasMany(x => x.Replies).WithOne(X => X.Author).OnDelete(DeleteBehavior.ClientCascade);
        });

        modelBuilder.Entity<Post>(eb =>
        {
            eb.HasOne(x => x.Author).WithMany(x => x.Posts).OnDelete(DeleteBehavior.ClientCascade);
            eb.HasMany(x => x.Likes).WithMany(x => x.PostLiked);
        });

        modelBuilder.Entity<Comment>(eb =>
        {
            eb.HasOne(x => x.Author).WithMany(x => x.Replies).OnDelete(DeleteBehavior.ClientCascade);
            eb.HasOne(x => x.Post).WithMany(x => x.Comments).OnDelete(DeleteBehavior.ClientCascade);
        });
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
}