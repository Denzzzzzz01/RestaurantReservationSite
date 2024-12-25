using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Cqrs.Restaurant.Commands.AddRestaurant;
using RRS.Application.Interfaces;
using RRS.Core.Models;

public class AddRestaurantCommandHandler : IRequestHandler<AddRestaurantCommand, Guid>
{
    private readonly IAppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public AddRestaurantCommandHandler(IAppDbContext dbContext, UserManager<AppUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<Guid> Handle(AddRestaurantCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                var isManager = await _dbContext.RestaurantManagerDatas
                    .AnyAsync(m => m.AppUserId == request.User.Id, cancellationToken);

                if (isManager)
                    throw new InvalidOperationException("User is already a manager of another restaurant.");

                var restaurant = request.Adapt<Restaurant>();
                restaurant.LogoUrl = "uploads/restaurants/_defaultrestaurantlogo.png";

                var restaurantManager = new RestaurantManagerData
                {
                    Id = Guid.NewGuid(),
                    AppUserId = request.User.Id,
                    Restaurant = restaurant
                };

                if (!(await _userManager.IsInRoleAsync(request.User, "RestaurantManager")))
                    await _userManager.AddToRoleAsync(request.User, "RestaurantManager");
                
                restaurant.Manageres.Add(restaurantManager);
                _dbContext.Restaurants.Add(restaurant);

                await _dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return restaurant.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

}
