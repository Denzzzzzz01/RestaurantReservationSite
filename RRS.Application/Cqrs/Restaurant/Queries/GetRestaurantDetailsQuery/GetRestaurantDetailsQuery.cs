using MediatR;
using RRS.Application.Contracts.Restaurant;

namespace RRS.Application.Cqrs.Restaurant.Queries.GetRestaurantDetailsQuery;

public record GetRestaurantDetailsQuery(Guid RestaurantId) : IRequest<RestaurantDto>;
