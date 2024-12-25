using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Contracts.Common;
using RRS.Application.Contracts.Restaurant;
using RRS.Application.Interfaces;

namespace RRS.Application.Cqrs.Restaurant.Queries.GetRestaurants;

public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, PagedResult<RestaurantSummaryDto>>
{
    private readonly IAppDbContext _dbContext;

    public GetRestaurantsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<RestaurantSummaryDto>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _dbContext.Restaurants.CountAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var restaurants = await _dbContext.Restaurants
            .OrderBy(r => r.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .Select(r => new RestaurantSummaryDto
            {
                Id = r.Id,
                Name = r.Name,
                Address = r.Address,
                LogoUrl = r.LogoUrl,
                OpeningHour = r.OpeningHour,
                ClosingHour = r.ClosingHour
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<RestaurantSummaryDto>
        {
            Items = restaurants,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

}

