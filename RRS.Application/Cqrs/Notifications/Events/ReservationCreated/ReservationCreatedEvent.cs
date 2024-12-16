using MediatR;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Events.ReservationCreated;

public record ReservationCreatedEvent(Core.Models.Restaurant Restaurant, AppUser User, DateTime ReservationDate, TimeSpan StartTime, TimeSpan EndTime) : INotification;
