using MediatR;
using RRS.Application.Interfaces;
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
        var message = $"Статус вашего бронирования изменен на: {notification.NewStatus}";

        var userNotification = new Notification
        {
            UserId = notification.User.Id,
            User = notification.User,
            RelatedEntityId = notification.RestaurantId,
            RelatedEntity = notification.RestaurantName,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        await _notificationRepository.AddNotificationAsync(userNotification, cancellationToken);
    }
}
