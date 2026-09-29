using System.ComponentModel.DataAnnotations;

namespace QrMenu.Models;

public class Category
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }


    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;


    public int DisplayOrder { get; set; } = 0;


    public bool IsDeleted { get; set; } = false;


    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;


    // Relationships

    public Restaurant Restaurant { get; set; } = null!;


    public ICollection<Product> Products { get; set; }
        = new List<Product>();
}