using MediatR;
using RRS.Application.Contracts.Notification;
using RRS.Application.Interfaces;

namespace RRS.Application.Cqrs.Notifications.Queries.GetUserRestaurants;

public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, IEnumerable<UserNotificationDto>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetUserNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<IEnumerable<UserNotificationDto>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _notificationRepository.GetUserNotificationsAsync(request.User.Id, cancellationToken);

        return notifications.Select(n => new UserNotificationDto
        {
            Id = n.Id,
            Message = n.Message,
            CreatedAt = n.CreatedAt,
            IsRead = n.IsRead,
            RelatedEntityId = n.RelatedEntityId,
            RelatedEntityName = n.RelatedEntityName,
            Type = n.Type
        });
    }
}

