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

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    private static string ConvertAvailabilityStatusToString(AvailabilityStatus status) => status switch
    {
        AvailabilityStatus.Available => "AVAILABLE",
        AvailabilityStatus.OutOfStock => "OUT_OF_STOCK",
        AvailabilityStatus.Seasonal => "SEASONAL",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    private static AvailabilityStatus ConvertStringToAvailabilityStatus(string value) => value switch
    {
        "AVAILABLE" => AvailabilityStatus.Available,
        "OUT_OF_STOCK" => AvailabilityStatus.OutOfStock,
        "SEASONAL" => AvailabilityStatus.Seasonal,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    private static string ConvertOrderStatusToString(OrderStatus status) => status switch
    {
        OrderStatus.Pending => "PENDING",
        OrderStatus.InPreparation => "IN_PREPARATION",
        OrderStatus.Cooking => "COOKING",
        OrderStatus.Ready => "READY",
        OrderStatus.Served => "SERVED",
        OrderStatus.Cancelled => "CANCELLED",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    private static OrderStatus ConvertStringToOrderStatus(string value) => value switch
    {
        "PENDING" => OrderStatus.Pending,
        "IN_PREPARATION" => OrderStatus.InPreparation,
        "COOKING" => OrderStatus.Cooking,
        "READY" => OrderStatus.Ready,
        "SERVED" => OrderStatus.Served,
        "CANCELLED" => OrderStatus.Cancelled,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

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
            .HasConversion(
                status => status == SessionStatus.Active
                    ? "ACTIVE"
                    : status == SessionStatus.Closed
                        ? "CLOSED"
                        : "EXPIRED",
                value => value == "ACTIVE"
                    ? SessionStatus.Active
                    : value == "CLOSED"
                        ? SessionStatus.Closed
                        : SessionStatus.Expired)
            .HasDefaultValue(SessionStatus.Active);


            entity.Property(s => s.SessionStart)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(s => s.SessionToken)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasIndex(s => s.SessionToken)
                .IsUnique();

            entity.Property(s => s.LastActivityAt);


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

            entity.Property(p => p.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(p => p.AvailabilityStatus)
                .HasConversion(
                    status => ConvertAvailabilityStatusToString(status),
                    value => ConvertStringToAvailabilityStatus(value));

            entity.HasIndex(p => p.RestaurantId);
            entity.HasIndex(p => p.CategoryId);

            entity.HasOne(p => p.Restaurant)
                .WithMany(r => r.Products)
                .HasForeignKey(p => p.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");

            entity.HasKey(o => o.Id);

        entity.Property(o => o.Status)
            .HasConversion(
                status => ConvertOrderStatusToString(status),
                value => ConvertStringToOrderStatus(value))
            .HasDefaultValue(OrderStatus.Pending);

            entity.Property(o => o.TotalAmount)
                .HasPrecision(10, 2)
                .HasDefaultValue(0m);

            entity.Property(o => o.IdempotencyKey)
                .HasMaxLength(100);

            entity.Property(o => o.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(o => o.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");


            // Indexes

            entity.HasIndex(o => new
                {
                    o.RestaurantId,
                    o.Status
                });


            entity.HasIndex(o => o.IdempotencyKey)
                .IsUnique();


            // Relationships

            entity.HasOne(o => o.Restaurant)
                .WithMany(r => r.Orders)
                .HasForeignKey(o => o.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);


            entity.HasOne(o => o.Table)
                .WithMany(t => t.Orders)
                .HasForeignKey(o => o.TableId)
                .OnDelete(DeleteBehavior.Restrict);


            entity.HasOne(o => o.Session)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.SessionId)
                .OnDelete(DeleteBehavior.SetNull);


            entity.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("order_items");

            entity.HasKey(i => i.Id);

            entity.Property(i => i.Quantity)
                .IsRequired();

            entity.Property(i => i.PriceSnapshot)
                .HasPrecision(10, 2)
                .IsRequired();


            entity.HasIndex(i => i.OrderId);


            entity.HasOne(i => i.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);


            entity.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}