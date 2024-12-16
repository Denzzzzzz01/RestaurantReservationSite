using MediatR;

namespace RRS.Application.Cqrs.Notifications.Commands.DeleteNotification;

public record DeleteNotificationCommand(Guid NotificationId, Guid UserId) : IRequest<Unit>;
