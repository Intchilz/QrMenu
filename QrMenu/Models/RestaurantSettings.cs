using System.ComponentModel.DataAnnotations;

namespace QrMenu.Models;

public class RestaurantSettings
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    // Theme
    [MaxLength(20)]
    public string PrimaryColor { get; set; } = "#6E2A2E";

    [MaxLength(20)]
    public string SecondaryColor { get; set; } = "#44161B";

    [MaxLength(50)]
    public string FontFamily { get; set; } = "Inter";

    // Menu
    [MaxLength(30)]
    public string MenuLayout { get; set; } = "GRID";

    // Orders
    public int OrderDelayThresholdMinutes { get; set; } = 20;

    // Table sessions
    public int SessionTimeoutMinutes { get; set; } = 120;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relationship
    public Restaurant Restaurant { get; set; } = null!;
}