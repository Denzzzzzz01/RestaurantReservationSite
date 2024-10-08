using MediatR;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Restaurant.Commands.UpdateRestaurant;

public record UpdateRestaurantCommand(
    AppUser User,
    Guid Id,
    string Name,
    Address Address,
    int SeatingCapacity,
    TimeSpan OpeningHour,
    TimeSpan ClosingHour,
    string? PhoneNumber,
    string? Website) : IRequest<Guid>;
