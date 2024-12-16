using MediatR;
using RRS.Application.Interfaces;

namespace RRS.Application.Cqrs.Notifications.Commands.DeleteNotification;

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, Unit>
{
    private readonly INotificationRepository _notificationRepository;

    public DeleteNotificationCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        await _notificationRepository.DeleteNotificationAsync(request.NotificationId, request.UserId, cancellationToken);
        return Unit.Value;
    }
}
