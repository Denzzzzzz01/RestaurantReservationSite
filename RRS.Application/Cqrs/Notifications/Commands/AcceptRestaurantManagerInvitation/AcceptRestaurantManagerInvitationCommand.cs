using MediatR;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Commands.AcceptRestaurantManagerInvitation;

public record AcceptRestaurantManagerInvitationCommand(Guid NotificationId, AppUser User) : IRequest<Unit>;
