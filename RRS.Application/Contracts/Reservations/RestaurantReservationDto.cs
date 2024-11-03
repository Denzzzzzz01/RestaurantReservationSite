using RRS.Core.Enums;

namespace RRS.Application.Contracts.Reservations;

public class RestaurantReservationDto
{
    public Guid Id { get; set; }
    public DateTime ReservationDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int NumberOfSeats { get; set; }
    public ReservationStatus Status { get; set; }
    public string UserName { get; set; }
    public string UserContact { get; set; }
    public Guid UserId{ get; set; }   
}
