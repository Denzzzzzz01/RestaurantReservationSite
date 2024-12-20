using MediatR;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Restaurant.Commands.DeleteRestaurant;

public record DeleteRestaurantCommand(Guid Id, AppUser User) : IRequest<Guid>;
