using RRS.Core.Models;

namespace RRS.Application.Interfaces;

public interface INotificationRepository
{
    Task AddNotificationAsync(Notification notification, CancellationToken cancellationToken);
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<Notification>> GetRetaurantNotificationsAsync(Guid userId, CancellationToken cancellationToken);
    Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken);
    Task DeleteNotificationAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken);

}
