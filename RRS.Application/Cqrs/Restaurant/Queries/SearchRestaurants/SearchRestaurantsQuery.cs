using MediatR;
using RRS.Application.Contracts.Restaurant;

namespace RRS.Application.Cqrs.Restaurant.Queries.SearchRestaurants;

public record SearchRestaurantsQuery(SearchRestaurantsDto SearchCriteria) : IRequest<List<RestaurantSummaryDto>>;
