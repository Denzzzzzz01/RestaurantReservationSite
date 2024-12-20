using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Interfaces;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Commands.SendRestaurantManagerInvitation;

public class SendRestaurantManagerInvitationCommandHandler : IRequestHandler<SendRestaurantManagerInvitationCommand, Guid>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IAppDbContext _dbContext;

    public SendRestaurantManagerInvitationCommandHandler(
        INotificationRepository notificationRepository,
        IAppDbContext dbContext)
    {
        _notificationRepository = notificationRepository;
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(SendRestaurantManagerInvitationCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _dbContext.Restaurants
            .FirstOrDefaultAsync(r => r.Id == request.RestaurantId, cancellationToken)
            ?? throw new InvalidOperationException("Restaurant not found.");

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = request.RecipientId,
            RelatedEntityId = restaurant.Id,
            RelatedEntityName = restaurant.Name,
            Message = $"{request.Sender.UserName} invites you to become a manager of {restaurant.Name}.",
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            Type = NotificationType.RestaurantInvitation
        };

        await _notificationRepository.AddNotificationAsync(notification, cancellationToken);
        return notification.Id;
    }
}
