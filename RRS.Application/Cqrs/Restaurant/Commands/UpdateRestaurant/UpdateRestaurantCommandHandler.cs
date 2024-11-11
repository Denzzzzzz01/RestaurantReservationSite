using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Interfaces;

namespace RRS.Application.Cqrs.Restaurant.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, Guid>
{
    private readonly IAppDbContext _dbContext;

    public UpdateRestaurantCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var isManager = await _dbContext.Restaurants
            .Where(r => r.Id == request.Id)
            .Select(r => r.Manageres.Any(m => m.AppUserId == request.User.Id))
            .FirstOrDefaultAsync(cancellationToken);

        if (!isManager)
            throw new InvalidOperationException("User is not a manager of this restaurant.");

        var affectedRows = await _dbContext.Restaurants
            .Where(r => r.Id == request.Id)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(r => r.Name, request.Name)
                .SetProperty(r => r.OpeningHour, request.OpeningHour)
                .SetProperty(r => r.ClosingHour, request.ClosingHour)
                .SetProperty(r => r.PhoneNumber, request.PhoneNumber)
                .SetProperty(r => r.Website, request.Website)

                .SetProperty(r => r.Address.Street, request.Address.Street)
                .SetProperty(r => r.Address.City, request.Address.City)
                .SetProperty(r => r.Address.Country, request.Address.Country)
                .SetProperty(r => r.Address.State, request.Address.State)
                .SetProperty(r => r.Address.Latitude, request.Address.Latitude)
                .SetProperty(r => r.Address.Longitude, request.Address.Longitude),
                cancellationToken);

        if (affectedRows == 0)
            throw new InvalidOperationException("Failed to update restaurant.");

        return request.Id;
    }
}


