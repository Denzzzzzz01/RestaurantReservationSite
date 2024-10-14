using Microsoft.AspNetCore.Identity;

namespace RRS.Core.Models;

public class AppUser : IdentityUser<Guid>
{ 
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    public List<Reservation> ReservationsHistory { get; set; } = new List<Reservation>();

    public bool isRestaurantManager { get; set; } = false;
    public RestaurantManagerData? RestaurantManagerData { get; set; } = null;
}
