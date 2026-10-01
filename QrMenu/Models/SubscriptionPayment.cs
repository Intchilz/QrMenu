using System.ComponentModel.DataAnnotations;

namespace QrMenu.Models;

public class SubscriptionPayment
{
    public Guid Id { get; set; }

    public Guid SubscriptionId { get; set; }

    public Guid RestaurantId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? TransactionReference { get; set; }

    [MaxLength(30)]
    public SubscriptionPaymentStatus Status { get; set; }
    = SubscriptionPaymentStatus.Pending;

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    // Relationships
    public Subscription Subscription { get; set; } = null!;

    public Restaurant Restaurant { get; set; } = null!;
}

public enum SubscriptionPaymentStatus
{
    Pending,
    Completed,
    Failed,
    Cancelled
}