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

    public DbSet<RestaurantTable> RestaurantTables => Set<RestaurantTable>();

    public DbSet<TableSession> TableSessions => Set<TableSession>();


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

        builder.Entity<RestaurantTable>(entity =>
        {
            entity.ToTable("tables");


            entity.HasKey(t => t.Id);


            entity.Property(t => t.TableName)
                .IsRequired()
                .HasMaxLength(100);


            entity.Property(t => t.QrToken)
                .IsRequired()
                .HasMaxLength(200);


            entity.Property(t => t.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            entity.HasIndex(t => t.QrToken)
                .IsUnique();


            entity.HasOne(t => t.Restaurant)
                .WithMany(r => r.Tables)
                .HasForeignKey(t => t.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // TableSession entity configuration
        builder.Entity<TableSession>(entity =>
        {
            entity.ToTable("table_sessions");


            entity.HasKey(s => s.Id);


            entity.Property(s => s.Status)
                .HasConversion<string>();


            entity.Property(s => s.SessionStart)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            entity.HasOne(s => s.Table)
                .WithMany(t => t.Sessions)
                .HasForeignKey(s => s.TableId)
                .OnDelete(DeleteBehavior.Cascade);


            entity.HasOne(s => s.Restaurant)
                .WithMany()
                .HasForeignKey(s => s.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}