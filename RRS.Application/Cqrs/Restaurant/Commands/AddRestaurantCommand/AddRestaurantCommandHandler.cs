using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Cqrs.Restaurant.Commands.AddRestaurantCommand;
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
        var isManager = await _dbContext.RestaurantManagerDatas
            .AnyAsync(m => m.AppUserId == request.User.Id, cancellationToken);

        if (isManager)
        {
            throw new InvalidOperationException("User is already a manager of another restaurant.");
        }

        var restaurant = new Restaurant
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Address = request.Address,
            SeatingCapacity = request.SeatingCapacity,
            OpeningHour = request.OpeningHour,
            ClosingHour = request.ClosingHour,
            PhoneNumber = request.PhoneNumber,
            Website = request.Website
        };

        var restaurantManager = new RestaurantManagerData
        {
            Id = Guid.NewGuid(),
            AppUserId = request.User.Id,
            Restaurant = restaurant
        };

        request.User.isRestaurantManager = true;
        await _userManager.UpdateAsync(request.User);

        restaurant.Manageres.Add(restaurantManager);
        _dbContext.Restaurants.Add(restaurant);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return restaurant.Id;
    }
}
