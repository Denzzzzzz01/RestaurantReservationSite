using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Contracts.Table;
using RRS.Application.Interfaces;

namespace RRS.Application.Cqrs.Table.Queries.GetRestaurantTables;

public class GetRestaurantTablesQueryHandler : IRequestHandler<GetRestaurantTablesQuery, List<RestaurantTableDto>>
{
    private readonly IAppDbContext _dbContext;

    public GetRestaurantTablesQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RestaurantTableDto>> Handle(GetRestaurantTablesQuery request, CancellationToken cancellationToken)
    {
        var restaurantExists = await _dbContext.Restaurants
            .AnyAsync(r => r.Id == request.RestaurantId, cancellationToken);

        if (!restaurantExists)
            throw new InvalidOperationException("Restaurant not found.");

        var tables = await _dbContext.RestaurantTables
            .Where(t => t.RestaurantId == request.RestaurantId)
            .OrderBy(t => t.TableNumber)
            .Select(t => new RestaurantTableDto
            {
                Id = t.Id,
                Capacity = t.Capacity,
                TableNumber = t.TableNumber,
                Description = t.Description,
                IsAvailable = t.IsAvailable
            })
            .ToListAsync(cancellationToken);

        return tables;
    }
}
