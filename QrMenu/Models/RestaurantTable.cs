namespace QrMenu.Models;

public class RestaurantTable
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    public string TableName { get; set; } = string.Empty;

    public string QrToken { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    // Relationships

    public Restaurant Restaurant { get; set; } = null!;

    public ICollection<TableSession> Sessions { get; set; }
        = new List<TableSession>();
}