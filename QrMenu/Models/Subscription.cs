using System.ComponentModel.DataAnnotations;

namespace QrMenu.Models;

public class Subscription
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    public SubscriptionStatus Status { get; set; }
        = SubscriptionStatus.Active;

    public DateTime ExpiryDate { get; set; }

    [MaxLength(50)]
    public string? PaymentMethod { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Restaurant Restaurant { get; set; } = null!;

    public ICollection<SubscriptionPayment> Payments { get; set; }
        = new List<SubscriptionPayment>();
}

public enum SubscriptionStatus
{
    Active,
    Expired
}