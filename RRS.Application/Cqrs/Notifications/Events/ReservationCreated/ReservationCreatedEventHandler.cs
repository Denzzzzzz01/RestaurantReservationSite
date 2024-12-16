using MediatR;
using RRS.Application.Interfaces;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Events.ReservationCreated;

public class ReservationCreatedEventHandler : INotificationHandler<ReservationCreatedEvent>
{
    private readonly INotificationRepository _notificationRepository;

    public ReservationCreatedEventHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(ReservationCreatedEvent notification, CancellationToken cancellationToken)
    {
        var userMessage = $"Your reservation at {notification.Restaurant.Name} on {notification.ReservationDate:yyyy-MM-dd} at {notification.StartTime:hh\\:mm} was successfully created.";
        var userNotification = new Notification
        {
            UserId = notification.User.Id,
            User = notification.User,
            RelatedEntityId = notification.Restaurant.Id,
            RelatedEntityName = notification.Restaurant.Name,
            Message = userMessage,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            Type = NotificationType.General
        };
        await _notificationRepository.AddNotificationAsync(userNotification, cancellationToken);

        var restaurantMessage = $"A new reservation was made by {notification.User.UserName} for {notification.ReservationDate:yyyy-MM-dd} at {notification.StartTime:hh\\:mm}.";
        var restaurantNotification = new Notification
        {
            RestaurantId = notification.Restaurant.Id,
            Restaurant = notification.Restaurant,
            RelatedEntityId = notification.User.Id,
            RelatedEntityName = notification.User.UserName,
            Message = restaurantMessage,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            Type = NotificationType.General
        };
        await _notificationRepository.AddNotificationAsync(restaurantNotification, cancellationToken);
    }
}

