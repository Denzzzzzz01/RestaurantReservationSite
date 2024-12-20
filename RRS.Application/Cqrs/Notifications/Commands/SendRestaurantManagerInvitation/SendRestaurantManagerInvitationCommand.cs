using MediatR;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Notifications.Commands.SendRestaurantManagerInvitation;

public record SendRestaurantManagerInvitationCommand(Guid RestaurantId, AppUser Sender, Guid RecipientId) : IRequest<Guid>;
