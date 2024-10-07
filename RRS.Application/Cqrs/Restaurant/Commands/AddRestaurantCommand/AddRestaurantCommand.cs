using MediatR;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Restaurant.Commands.AddRestaurantCommand;

public record AddRestaurantCommand
(
    AppUser User,
    string Name,
    Address Address,
    int SeatingCapacity,
    int OpeningHour,
    int ClosingHour,
    string? PhoneNumber,
    string? Website
) : IRequest<Guid>;
