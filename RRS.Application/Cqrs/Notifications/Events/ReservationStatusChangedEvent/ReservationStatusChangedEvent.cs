using MediatR;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Events.ReservationStatusChangedEvent;

public record ReservationStatusChangedEvent(Guid RestaurantId, string RestaurantName, AppUser User, ReservationStatus NewStatus) : INotification;

