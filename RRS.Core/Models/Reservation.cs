using RRS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace RRS.Core.Models;

public class Reservation
{
    public Guid Id { get; set; }

    [Required]
    public Guid RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }

    [Required]
    public Guid UserId { get; set; }    
    public AppUser User { get; set; }

    [Required]
    public DateTime ReservationDate { get; set; }

    [Required]
    [Range(1, 20)]
    public int NumberOfSeats { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Active;
    public bool IsCompleted { get; set; } = false;
}
