using System.ComponentModel.DataAnnotations;

namespace QrMenu.Models;

public class Coupon
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal DiscountValue { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? CategoryId { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public Restaurant Restaurant { get; set; } = null!;

    public Product? Product { get; set; }

    public Category? Category { get; set; }
}