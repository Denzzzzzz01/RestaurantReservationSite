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
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
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

                int maxTableNumber = restaurant.Tables.Any() ? restaurant.Tables.Max(t => t.TableNumber) : 0;

                var newTables = new List<RestaurantTable>();
                for (int i = 0; i < request.NumberOfTables; i++)
                {
                    var newTable = new RestaurantTable
                    {
                        Id = Guid.NewGuid(),
                        RestaurantId = restaurant.Id,   
                        Restaurant = restaurant,
                        Capacity = request.TableCapacity,
                        Description = request.Description,
                        IsAvailable = true,
                        TableNumber = ++maxTableNumber
                    };

                    newTables.Add(newTable);
                }

                _dbContext.RestaurantTables.AddRange(newTables);
                await _dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync();

                return Unit.Value;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}


