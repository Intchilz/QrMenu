using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QrMenu.Models;

namespace QrMenu.Data;

public class ApplicationDbContext 
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    public DbSet<Restaurant> Restaurants => Set<Restaurant>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<Restaurant>(entity =>
        {
            entity.ToTable("restaurants");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(200);


            entity.Property(r => r.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            entity.HasMany(r => r.Users)
                .WithOne(u => u.Restaurant)
                .HasForeignKey(u => u.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.FirstName)
                .HasMaxLength(100);

            entity.Property(u => u.LastName)
                .HasMaxLength(100);
        });
    }
}