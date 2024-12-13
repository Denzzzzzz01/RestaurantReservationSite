using Microsoft.AspNetCore.Identity;

namespace RRS.Core.Models;

public class AppUser : IdentityUser<Guid>
{ 
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    public List<Notification> Notifications { get; set; } = new List<Notification>();

    public RestaurantManagerData? RestaurantManagerData { get; set; } = null;
}
