using RRS.Core.Enums;

namespace RRS.Application.Contracts.Reservations;

public class UserReservationDto
{
    public Guid Id { get; set; }
    public DateTime ReservationDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int NumberOfSeats { get; set; }
    public ReservationStatus Status { get; set; }
    public string RestaurantName { get; set; }
    public Guid RestaurantId { get; set; }
}
