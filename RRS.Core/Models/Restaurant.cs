using System.ComponentModel.DataAnnotations;

namespace RRS.Core.Models;

public class Restaurant
{
    public Guid Id { get; set; }

    [Required]
    public List<RestaurantManagerData> Manageres { get; set; } = new List<RestaurantManagerData>();

    [Required]
    [StringLength(100)]
    public string Name { get; set; }  

    [Required]
    [StringLength(200)]
    public Address Address { get; set; }

    [Required]
    [Range(0, 24)]
    public int OpeningHour { get; set; } = 0;

    [Required]
    [Range(0, 24)]
    public int ClosingHour { get; set; } = 24;

    [Required]
    [Range(1, 500, ErrorMessage = "Seating capacity must be between 1 and 500.")]
    public int SeatingCapacity { get; set; }

    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(100)]
    public string? Website { get; set; }
}
