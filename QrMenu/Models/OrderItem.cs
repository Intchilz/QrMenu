using System.ComponentModel.DataAnnotations;

namespace QrMenu.Models;

public class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PriceSnapshot { get; set; }


    // Relationships

    public Order Order { get; set; } = null!;

    public Product Product { get; set; } = null!;
}