using MediatR;

namespace RRS.Application.Cqrs.TableBooking.Commands.BookTable;

public record BookTableCommand(
    Guid RestaurantId, 
    DateTime ReservationDate, 
    TimeSpan StartTime, 
    int NumberOfSeats, 
    Guid UserId) : IRequest<Guid>;
