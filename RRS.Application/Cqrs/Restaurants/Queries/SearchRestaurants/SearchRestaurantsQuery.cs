using MediatR;
using RRS.Application.Contracts.Common;
using RRS.Application.Contracts.Restaurant;

namespace RRS.Application.Cqrs.Restaurant.Queries.SearchRestaurants;

public record SearchRestaurantsQuery(SearchRestaurantsDto SearchCriteria, int PageNumber, int PageSize) : IRequest<PagedResult<RestaurantSummaryDto>>;
