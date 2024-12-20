using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Contracts.Restaurant;
using RRS.Application.Interfaces;

namespace RRS.Application.Cqrs.Restaurant.Queries.GetRestaurantDetailsQuery;

public class GetRestaurantDetailsQueryHandler : IRequestHandler<GetRestaurantDetailsQuery, RestaurantDto>
{
    private readonly IAppDbContext _dbContext;
    public GetRestaurantDetailsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<RestaurantDto> Handle(GetRestaurantDetailsQuery request, CancellationToken cancellationToken)
    {

        var restaurant = await _dbContext.Restaurants
            .AsNoTracking()
            .Include(r => r.Manageres)
            .ThenInclude(m => m.AppUser)
            .FirstOrDefaultAsync(r => r.Id == request.RestaurantId, cancellationToken);

        
        return restaurant.Adapt<RestaurantDto>();
    }
}



