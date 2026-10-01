namespace QrMenu.Services;

public interface ICurrentTenant
{
    Guid RestaurantId { get; }

    bool IsAvailable { get; }
}