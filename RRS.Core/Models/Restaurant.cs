using System.ComponentModel.DataAnnotations;

namespace RRS.Core.Models;

public class Restaurant
{
    public Guid Id { get; set; }

    [Required]
    public List<RestaurantManagerData> Manageres { get; set; } = new List<RestaurantManagerData>();
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    public List<RestaurantTable> Tables { get; set; } = new List<RestaurantTable>();
    public List<Notification> Notifications { get; set; } = new List<Notification>();

    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [StringLength(1000)]
    public string Description { get; set; } = String.Empty;

    [Required]
    public Address Address { get; set; }
    public string LogoUrl { get; set; }

    [Required]
    public TimeSpan OpeningHour { get; set; } = TimeSpan.FromHours(0);

    [Required]
    public TimeSpan ClosingHour { get; set; } = TimeSpan.FromHours(24);

    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(100)]
    public string? Website { get; set; }

}
