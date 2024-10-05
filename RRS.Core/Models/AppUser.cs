using Microsoft.AspNetCore.Identity;

namespace RRS.Core.Models;

public class AppUser : IdentityUser<Guid>
{

    public RestaurantManagerData? RestaurantManagerData { get; set; } = null;
}
