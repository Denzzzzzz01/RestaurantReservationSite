using MediatR;
using RRS.Application.Contracts.Notification;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Queries.GetRestaurantNotifications;

public record GetRestaurantNotificationsQuery(AppUser User) : IRequest<IEnumerable<RestaurantNotificationDto>>;
