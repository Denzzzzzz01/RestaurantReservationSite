using Microsoft.AspNetCore.Identity;

namespace RRS.Core.Models;

public class AppUser : IdentityUser<Guid>
{
    public bool isRestaurantManager { get; set; } = false;
    public RestaurantManagerData? RestaurantManagerData { get; set; } = null;
}
