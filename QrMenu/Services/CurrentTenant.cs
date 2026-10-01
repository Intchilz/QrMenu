using System.Security.Claims;

namespace QrMenu.Services;

public class CurrentTenant : ICurrentTenant
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentTenant(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid RestaurantId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User.FindFirstValue("RestaurantId");

            if (!Guid.TryParse(value, out var restaurantId))
            {
                throw new InvalidOperationException(
                    "No restaurant is associated with the current user.");
            }

            return restaurantId;
        }
    }

    public bool IsAvailable
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User.FindFirstValue("RestaurantId");

            return Guid.TryParse(value, out _);
        }
    }
}