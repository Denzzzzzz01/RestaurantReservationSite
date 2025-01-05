using System.ComponentModel.DataAnnotations;

namespace RRS.Application.Contracts.Reservations;

public class BookTableDto
{
    [Required]
    public Guid RestaurantId { get; set; }

    [Required]
    public DateTime ReservationDate { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    [Range(1, 500, ErrorMessage = "Number of seats must be between 1 and 500.")]
    public int NumberOfSeats { get; set; }
}
