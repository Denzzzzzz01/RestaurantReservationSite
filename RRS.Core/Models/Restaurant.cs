using System.ComponentModel.DataAnnotations;

namespace RRS.Core.Models;

public class Restaurant
{
    public Guid Id { get; set; }

    [Required]
    public List<RestaurantManagerData> Manageres { get; set; } = new List<RestaurantManagerData>();
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    public List<Reservation> ReservationHistory { get; set; } = new List<Reservation>();


    [Required]
    [StringLength(100)]
    public string Name { get; set; }  

    [Required]
    public Address Address { get; set; }

    [Required]
    public TimeSpan OpeningHour { get; set; } = TimeSpan.FromHours(0);

    [Required]
    public TimeSpan ClosingHour { get; set; } = TimeSpan.FromHours(24);

    [Required]
    [Range(1, 500, ErrorMessage = "Seating capacity must be between 1 and 500.")]
    public int SeatingCapacity { get; set; }

    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(100)]
    public string? Website { get; set; }
}
