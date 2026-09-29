using System.ComponentModel.DataAnnotations;

namespace QrMenu.Models;

public class Product
{
    public Guid Id { get; set; }


    public Guid RestaurantId { get; set; }


    public Guid CategoryId { get; set; }


    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;


    public string? Description { get; set; }


    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }


    public string? ImageUrl { get; set; }


    public AvailabilityStatus AvailabilityStatus { get; set; }
        = AvailabilityStatus.Available;


    public bool IsDeleted { get; set; } = false;


    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;



    // Relationships

    public Restaurant Restaurant { get; set; } = null!;


    public Category Category { get; set; } = null!;
}



public enum AvailabilityStatus
{
    Available,
    OutOfStock,
    Seasonal
}