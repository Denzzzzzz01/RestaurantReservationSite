using MediatR;
using RRS.Application.Interfaces;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Events.ReservationCancelled;

public class ReservationCancelledEventHandler : INotificationHandler<ReservationCancelledEvent>
{
    private readonly INotificationRepository _notificationRepository;

    public ReservationCancelledEventHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(ReservationCancelledEvent notification, CancellationToken cancellationToken)
    {
        var message = $"Reservation was cancelled by user: {notification.User.UserName}.";

        var restaurantNotification = new Notification
        {
            RestaurantId = notification.Restaurant.Id,
            Restaurant = notification.Restaurant,
            RelatedEntityId = notification.User.Id,
            RelatedEntityName = notification.User.UserName,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            Type = NotificationType.General
        };

        await _notificationRepository.AddNotificationAsync(restaurantNotification, cancellationToken);
    }
}
