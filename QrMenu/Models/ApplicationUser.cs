using Microsoft.AspNetCore.Identity;

namespace QrMenu.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid? RestaurantId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    // Relationship

    public Restaurant? Restaurant { get; set; }

    public ICollection<Order> Orders { get; set; }
        = new List<Order>();
}