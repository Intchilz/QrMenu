namespace QrMenu.Models;

public class TableSession
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    public Guid TableId { get; set; }


    public SessionStatus Status { get; set; }
        = SessionStatus.Active;


    public DateTime SessionStart { get; set; }
        = DateTime.UtcNow;


    public DateTime? SessionEnd { get; set; }


    // Relationships

    public Restaurant Restaurant { get; set; } = null!;

    public RestaurantTable Table { get; set; } = null!;

    public ICollection<Order> Orders { get; set; }
        = new List<Order>();
}


public enum SessionStatus
{
    Active,
    Closed,
    Expired
}