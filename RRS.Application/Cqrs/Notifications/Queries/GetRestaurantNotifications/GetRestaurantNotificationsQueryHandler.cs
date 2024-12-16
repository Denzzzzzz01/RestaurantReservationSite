using MediatR;
using RRS.Application.Contracts.Notification;
using RRS.Application.Interfaces;

namespace RRS.Application.Cqrs.Notifications.Queries.GetRestaurantNotifications;

public class GetRestaurantNotificationsQueryHandler : IRequestHandler<GetRestaurantNotificationsQuery, IEnumerable<RestaurantNotificationDto>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetRestaurantNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<IEnumerable<RestaurantNotificationDto>> Handle(GetRestaurantNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _notificationRepository.GetRetaurantNotificationsAsync(request.User.Id, cancellationToken);

        return notifications.Select(n => new RestaurantNotificationDto
        {
            Id = n.Id,
            Message = n.Message,
            CreatedAt = n.CreatedAt,
            RelatedEntityId = n.RelatedEntityId,
            RelatedEntityName = n.RelatedEntityName,
            Type = n.Type
        });
    }

}
