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
            eb.HasMany<Post>().WithOne(x => x.Author);
        });
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
}