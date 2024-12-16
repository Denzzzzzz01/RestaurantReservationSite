using MediatR;
using RRS.Application.Interfaces;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Events.ReservationStatusChangedEvent;

public class ReservationStatusChangedEventHandler : INotificationHandler<ReservationStatusChangedEvent>
{
    private readonly INotificationRepository _notificationRepository;

    public ReservationStatusChangedEventHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(ReservationStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        var message = $"Your reservetaion status was changed to: {notification.NewStatus}";

        if (notification.User is null)
            throw new Exception("User not found for the reservation.");
        
        var userNotification = new Notification
        { 
            UserId = notification.User.Id,
            User = notification.User,
            RelatedEntityId = notification.RestaurantId,
            RelatedEntityName = notification.RestaurantName,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            Type = NotificationType.General
        };

        await _notificationRepository.AddNotificationAsync(userNotification, cancellationToken);
    }
}
