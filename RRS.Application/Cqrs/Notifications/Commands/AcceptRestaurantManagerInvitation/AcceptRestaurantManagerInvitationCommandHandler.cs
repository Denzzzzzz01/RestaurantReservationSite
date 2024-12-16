using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Interfaces;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Commands.AcceptRestaurantManagerInvitation;

public class AcceptRestaurantManagerInvitationCommandHandler : IRequestHandler<AcceptRestaurantManagerInvitationCommand, Unit>
{
    private readonly IAppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public AcceptRestaurantManagerInvitationCommandHandler(
        IAppDbContext dbContext,
        UserManager<AppUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(AcceptRestaurantManagerInvitationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _dbContext.Notifications
            .FirstOrDefaultAsync(n => n.Id == request.NotificationId && n.UserId == request.User.Id, cancellationToken)
            ?? throw new InvalidOperationException("Invitation not found.");

        if (!notification.RestaurantId.HasValue)
            throw new InvalidOperationException("The invitation does not contain a valid restaurant reference.");

        var restaurant = await _dbContext.Restaurants
            .FirstOrDefaultAsync(r => r.Id == notification.RestaurantId, cancellationToken)
            ?? throw new InvalidOperationException("Restaurant not found.");

        var isManager = await _dbContext.RestaurantManagerDatas
            .AnyAsync(m => m.AppUserId == request.User.Id, cancellationToken);

        if (isManager)
            throw new InvalidOperationException("User is already a manager of another restaurant.");

        var restaurantManager = new RestaurantManagerData
        {
            Id = Guid.NewGuid(),
            AppUserId = request.User.Id,
            RestaurantId = restaurant.Id
        };

        _dbContext.RestaurantManagerDatas.Add(restaurantManager);

        if (!await _userManager.IsInRoleAsync(request.User, "RestaurantManager"))
            await _userManager.AddToRoleAsync(request.User, "RestaurantManager");

        notification.IsRead = true;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
