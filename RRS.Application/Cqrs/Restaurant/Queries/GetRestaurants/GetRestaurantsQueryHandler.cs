using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Contracts.AppUser;
using RRS.Application.Contracts.Restaurant;
using RRS.Application.Interfaces;

namespace RRS.Application.Cqrs.Restaurant.Queries.GetRestaurants;

public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, List<RestaurantSummaryDto>>
{
    private readonly IAppDbContext _dbContext;

    public GetRestaurantsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RestaurantSummaryDto>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var restaurants = await _dbContext.Restaurants
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .Select(r => new RestaurantSummaryDto
            {
                Id = r.Id,
                Name = r.Name,
                Address = r.Address,
                SeatingCapacity = r.SeatingCapacity,
                OpeningHour = r.OpeningHour,
                ClosingHour = r.ClosingHour
            })
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken); 

        return restaurants;
    }
}

