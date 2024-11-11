using System.ComponentModel.DataAnnotations;

namespace RRS.Core.Models;

public class RestaurantTable
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid RestaurantId { get; set; }
    [Required]
    public Restaurant Restaurant { get; set; }

    [Required]
    [Range(1, 20, ErrorMessage = "Table capacity must be between 1 and 20.")]
    public int Capacity { get; set; }

    [Required]
    public int TableNumber { get; set; }

    public string Description { get; set; } 

    public bool IsAvailable { get; set; } = true;  

    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
}
