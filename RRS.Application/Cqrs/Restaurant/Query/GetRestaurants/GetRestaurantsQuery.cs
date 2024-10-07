
using MediatR;
using RRS.Application.Contracts.Restaurant;

namespace RRS.Application.Cqrs.Restaurant.Query.GetRestaurants;

public record GetRestaurantsQuery : IRequest<List<RestaurantDto>>;
