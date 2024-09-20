using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Model;

namespace WebApp.Data;

public class DataContext(DbContextOptions options): IdentityDbContext<User>(options)
{
    public DbSet<Friend> Friends { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Post>()
            .HasOne(x => x.Author)
            .WithMany(x => x.Posts)
            .IsRequired();
        builder.Entity<Post>()
            .HasMany(x => x.Comments)
            .WithOne(x => x.Post).IsRequired();
    }

}