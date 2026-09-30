using System.ComponentModel.DataAnnotations;

namespace QrMenu.Models;

public class AuditLog
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    public Guid? UserId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Action { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Target { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Relationships
    public Restaurant Restaurant { get; set; } = null!;

    public ApplicationUser? User { get; set; }
}