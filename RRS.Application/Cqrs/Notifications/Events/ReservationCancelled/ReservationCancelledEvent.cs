using MediatR;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Events.ReservationCancelled;

public record ReservationCancelledEvent(Core.Models.Restaurant Restaurant, AppUser User, ReservationStatus NewStatus) : INotification;
