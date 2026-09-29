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
}