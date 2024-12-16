using RRS.Core.Enums;

namespace RRS.Application.Contracts.Notification;

public class RestaurantNotificationDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public string RelatedEntityName { get; set; }
    public NotificationType Type { get; set; }
}
