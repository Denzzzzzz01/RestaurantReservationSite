using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Interfaces;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Restaurant.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, Guid>
{
    private readonly IAppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public DeleteRestaurantCommandHandler(IAppDbContext dbContext, UserManager<AppUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _dbContext.Restaurants
            .AsNoTracking()
            .Where(r => r.Id == request.Id)
            //.Where(r => r.Manageres.Any(m => m.AppUserId == request.User.Id))
            .Include(r => r.Manageres)
            .FirstOrDefaultAsync(cancellationToken);

        if (restaurant is null)
            throw new InvalidOperationException("Restaurant not found.");

        var affectedRows = await _dbContext.Restaurants
            .Where(r => r.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (affectedRows == 0)
            throw new InvalidOperationException("Failed to delete restaurant.");

        var managerIds = restaurant.Manageres.Select(m => m.AppUserId).ToList();
        var managers = await _userManager.Users
            .Where(u => managerIds.Contains(u.Id))
            .ToListAsync(cancellationToken);
        foreach (var manager in managers)
        {
            manager.isRestaurantManager = false;
            await _userManager.UpdateAsync(manager);
        }

        return request.Id;
    }
}



