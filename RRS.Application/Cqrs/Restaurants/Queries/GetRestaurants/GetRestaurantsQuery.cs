
using MediatR;
using RRS.Application.Contracts.Common;
using RRS.Application.Contracts.Restaurant;

namespace RRS.Application.Cqrs.Restaurant.Queries.GetRestaurants;

public record GetRestaurantsQuery(int PageNumber, int PageSize) : IRequest<PagedResult<RestaurantSummaryDto>>; 
