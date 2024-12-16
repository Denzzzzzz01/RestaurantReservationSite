using MediatR;
using RRS.Application.Contracts.Notification;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Queries.GetUserRestaurants;

public record GetUserNotificationsQuery(AppUser User) : IRequest<IEnumerable<UserNotificationDto>>;
