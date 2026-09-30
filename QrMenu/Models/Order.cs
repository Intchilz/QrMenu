using System.ComponentModel.DataAnnotations;

namespace QrMenu.Models;

public class Order
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    public Guid TableId { get; set; }

    public Guid? SessionId { get; set; }

    public Guid? UserId { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; } = 0m;

    [MaxLength(100)]
    public string? IdempotencyKey { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    // Relationships

    public Restaurant Restaurant { get; set; } = null!;

    public RestaurantTable Table { get; set; } = null!;

    public TableSession? Session { get; set; }

    public ApplicationUser? User { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; }
        = new List<OrderItem>();
}


public enum OrderStatus
{
    Pending,
    InPreparation,
    Cooking,
    Ready,
    Served,
    Cancelled
}