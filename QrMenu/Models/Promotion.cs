using System.ComponentModel.DataAnnotations;

namespace QrMenu.Models;

public class Promotion
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? PromotionalPrice { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationship
    public Restaurant Restaurant { get; set; } = null!;
}