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

    public DbSet<Coupon> Coupons => Set<Coupon>();

    public DbSet<Promotion> Promotions => Set<Promotion>();

    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    public DbSet<SubscriptionPayment> SubscriptionPayments
        => Set<SubscriptionPayment>();

    public DbSet<Notification> Notifications
        => Set<Notification>();

    public DbSet<AuditLog> AuditLogs
        => Set<AuditLog>();

    public DbSet<RestaurantSettings> RestaurantSettings
        => Set<RestaurantSettings>();

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

    private static string ConvertSubscriptionStatusToString(SubscriptionStatus status) => status switch
    {
        SubscriptionStatus.Active => "ACTIVE",
        SubscriptionStatus.Expired => "EXPIRED",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    private static SubscriptionStatus ConvertStringToSubscriptionStatus(string value) => value switch
    {
        "ACTIVE" => SubscriptionStatus.Active,
        "EXPIRED" => SubscriptionStatus.Expired,
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
    };

    private static string ConvertSubscriptionPaymentStatusToString(SubscriptionPaymentStatus status) => status switch
    {
        SubscriptionPaymentStatus.Pending => "PENDING",
        SubscriptionPaymentStatus.Completed => "COMPLETED",
        SubscriptionPaymentStatus.Failed => "FAILED",
        SubscriptionPaymentStatus.Cancelled => "CANCELLED",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };

    private static SubscriptionPaymentStatus ConvertStringToSubscriptionPaymentStatus(string value) => value switch
    {
        "PENDING" => SubscriptionPaymentStatus.Pending,
        "COMPLETED" => SubscriptionPaymentStatus.Completed,
        "FAILED" => SubscriptionPaymentStatus.Failed,
        "CANCELLED" => SubscriptionPaymentStatus.Cancelled,
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

        builder.Entity<Coupon>(entity =>
        {
            entity.ToTable("coupons");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(c => c.Type)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(c => c.DiscountValue)
                .HasPrecision(10, 2);

            entity.Property(c => c.ExpiryDate)
                .IsRequired();

            entity.Property(c => c.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(c => new { c.RestaurantId, c.Code })
                .IsUnique();

            entity.HasIndex(c => c.RestaurantId);

            entity.HasOne(c => c.Restaurant)
                .WithMany(r => r.Coupons)
                .HasForeignKey(c => c.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Promotion>(entity =>
        {
            entity.ToTable("promotions");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.PromotionalPrice)
                .HasPrecision(10, 2);

            entity.Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(p => new { p.RestaurantId, p.IsActive });

            entity.HasOne(p => p.Restaurant)
                .WithMany(r => r.Promotions)
                .HasForeignKey(p => p.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Subscription>(entity =>
        {
            entity.ToTable("subscriptions");

            entity.HasKey(s => s.Id);

            entity.Property(s => s.Status)
                .HasConversion(
                    status => ConvertSubscriptionStatusToString(status),
                    value => ConvertStringToSubscriptionStatus(value))
                .HasDefaultValue(SubscriptionStatus.Active);

            entity.Property(s => s.PaymentMethod)
                .HasMaxLength(50);

            entity.Property(s => s.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(s => new { s.RestaurantId, s.Status });

            entity.HasOne(s => s.Restaurant)
                .WithMany(r => r.Subscriptions)
                .HasForeignKey(s => s.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<SubscriptionPayment>(entity =>
        {
            entity.ToTable("subscription_payments");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Amount)
                .HasPrecision(10, 2);

            entity.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.TransactionReference)
                .HasMaxLength(100);

            entity.Property(p => p.Status)
                .HasConversion(
                    status => ConvertSubscriptionPaymentStatusToString(status),
                    value => ConvertStringToSubscriptionPaymentStatus(value))
                .HasDefaultValue(SubscriptionPaymentStatus.Pending);

            entity.Property(p => p.PaymentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(p => p.RestaurantId);
            entity.HasIndex(p => p.SubscriptionId);
            entity.HasIndex(p => p.TransactionReference);

            entity.HasOne(p => p.Restaurant)
                .WithMany(r => r.SubscriptionPayments)
                .HasForeignKey(p => p.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Subscription)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");

            entity.HasKey(n => n.Id);

            entity.Property(n => n.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(n => n.Message)
                .IsRequired();

            entity.Property(n => n.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(n => n.RestaurantId);

            entity.HasIndex(n => new { n.UserId, n.IsRead });

            entity.HasIndex(n => n.OrderId);

            entity.HasOne(n => n.Restaurant)
                .WithMany(r => r.Notifications)
                .HasForeignKey(n => n.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(n => n.Order)
                .WithMany()
                .HasForeignKey(n => n.OrderId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("audit_logs");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(a => a.Target)
                .HasMaxLength(200);

            entity.Property(a => a.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(a => a.RestaurantId);
            entity.HasIndex(a => a.UserId);
            entity.HasIndex(a => a.Timestamp);

            entity.HasOne(a => a.Restaurant)
                .WithMany(r => r.AuditLogs)
                .HasForeignKey(a => a.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<RestaurantSettings>(entity =>
        {
            entity.ToTable("restaurant_settings");

            entity.HasKey(s => s.Id);

            entity.HasIndex(s => s.RestaurantId)
                .IsUnique();

            entity.Property(s => s.PrimaryColor)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(s => s.SecondaryColor)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(s => s.FontFamily)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(s => s.MenuLayout)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(s => s.OrderDelayThresholdMinutes)
                .HasDefaultValue(20);

            entity.Property(s => s.SessionTimeoutMinutes)
                .HasDefaultValue(120);

            entity.Property(s => s.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(s => s.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(s => s.Restaurant)
                .WithOne(r => r.Settings)
                .HasForeignKey<RestaurantSettings>(s => s.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}