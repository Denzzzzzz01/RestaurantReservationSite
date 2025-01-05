using RRS.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace RRS.Application.Contracts.Restaurant;

public class AddRestaurantDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public Address Address { get; set; }
    public string Description { get; set; } = String.Empty;

    [Required]
    public string OpeningHour { get; set; }

    [Required]
    public string ClosingHour { get; set; }

    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(100)]
    public string? Website { get; set; }
}
