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

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Product> Products => Set<Product>();


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

        builder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");


            entity.HasKey(c => c.Id);


            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);


            entity.Property(c => c.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            entity.HasIndex(c => c.RestaurantId);


            entity.HasOne(c => c.Restaurant)
                .WithMany(r => r.Categories)
                .HasForeignKey(c => c.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Product>(entity =>
        {
            entity.ToTable("products");


            entity.HasKey(p => p.Id);


            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);


            entity.Property(p => p.Price)
                .HasPrecision(10, 2);


            entity.Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            entity.Property(p => p.AvailabilityStatus)
                .HasConversion<string>();


            entity.HasIndex(p => p.RestaurantId);


            entity.HasIndex(p => p.CategoryId);


            entity.HasOne(p => p.Restaurant)
                .WithMany(r => r.Products)
                .HasForeignKey(p => p.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);


            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}