namespace QrMenu.Models;

public class Restaurant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? LogoUrl { get; set; }

    public string? CoverImageUrl { get; set; }

    public string? ThemeConfig { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    // Relationships

    public ICollection<ApplicationUser> Users { get; set; }
        = new List<ApplicationUser>();

    public ICollection<RestaurantTable> Tables { get; set; }
    = new List<RestaurantTable>();

    public ICollection<Category> Categories { get; set; }
    = new List<Category>();


    public ICollection<Product> Products { get; set; }
        = new List<Product>();

    public ICollection<Order> Orders { get; set; }
        = new List<Order>(); 

    public ICollection<Coupon> Coupons { get; set; }
    = new List<Coupon>();

    public ICollection<Promotion> Promotions { get; set; }
        = new List<Promotion>();

    public ICollection<Subscription> Subscriptions { get; set; }
        = new List<Subscription>();

    public ICollection<SubscriptionPayment> SubscriptionPayments { get; set; }
        = new List<SubscriptionPayment>();

    public ICollection<Notification> Notifications { get; set; }
        = new List<Notification>();

    public ICollection<AuditLog> AuditLogs { get; set; }
        = new List<AuditLog>();       
    }