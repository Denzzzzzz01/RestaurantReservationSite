using MediatR;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Commands.BookTable;

public record BookTableCommand(
    Guid RestaurantId, 
    DateTime ReservationDate, 
    TimeSpan StartTime, 
    int NumberOfSeats, 
    AppUser User) : IRequest<Guid>;
