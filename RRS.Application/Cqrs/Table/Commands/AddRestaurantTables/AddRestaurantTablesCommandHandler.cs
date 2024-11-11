using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Interfaces;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Table.Commands.AddRestaurantTables;

public class AddRestaurantTablesCommandHandler : IRequestHandler<AddRestaurantTablesCommand, Unit>
{
    private readonly IAppDbContext _dbContext;

    public AddRestaurantTablesCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(AddRestaurantTablesCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _dbContext.Restaurants
            .Include(r => r.Manageres)
            .Include(r => r.Tables)
            .FirstOrDefaultAsync(r => r.Id == request.RestaurantId, cancellationToken);

        if (restaurant is null)
            throw new InvalidOperationException("Restaurant not found.");

        var isManager = restaurant.Manageres.Any(m => m.AppUserId == request.User.Id);

        if (!isManager)
            throw new InvalidOperationException("User is not a manager of this restaurant.");


        int maxTableNumber = restaurant.Tables.Any() ? restaurant.Tables.Max(t => t.TableNumber) : 1;

        var newTables = new List<RestaurantTable>();
        for (int i = 0; i < request.NumberOfTables; i++)
        {
            var newTable = new RestaurantTable
            {
                Id = Guid.NewGuid(),
                Capacity = request.TableCapacity,
                Description = request.Description,
                IsAvailable = true,
                TableNumber = ++maxTableNumber
            };

            newTables.Add(newTable);
        }

        restaurant.Tables.AddRange(newTables);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}


