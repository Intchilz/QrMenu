using System.ComponentModel.DataAnnotations;

namespace QrMenu.Models;

public class Notification
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    public Guid? UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    public Guid? OrderId { get; set; }

    public bool IsRead { get; set; } = false;

    public DateTime? ReadAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Restaurant Restaurant { get; set; } = null!;

    public ApplicationUser? User { get; set; }

    public Order? Order { get; set; }
}