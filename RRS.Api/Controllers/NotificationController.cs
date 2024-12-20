using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRS.Application.Contracts.Notification;
using RRS.Application.Cqrs.Notifications.Commands.AcceptRestaurantManagerInvitation;
using RRS.Application.Cqrs.Notifications.Commands.DeleteNotification;
using RRS.Application.Cqrs.Notifications.Commands.SendRestaurantManagerInvitation;
using RRS.Application.Cqrs.Notifications.Queries.GetRestaurantNotifications;
using RRS.Application.Cqrs.Notifications.Queries.GetUserRestaurants;
using RRS.Core.Models;

namespace RRS.Api.Controllers;

public class NotificationController : BaseController
{
    private readonly IMediator _mediator;

    public NotificationController(UserManager<AppUser> userManager, IMediator mediator) : base(userManager)
    {
        _mediator = mediator;
    }

    [HttpGet("user")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserNotificationDto>>> GetUserNotifications(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(); 
        var query = new GetUserNotificationsQuery(user);
        var notifications = await _mediator.Send(query, cancellationToken);

        return Ok(notifications);
    }

    [HttpGet("restaurant")]
    [Authorize(Roles = "RestaurantManager")]
    public async Task<ActionResult<IEnumerable<RestaurantNotificationDto>>> GetRestaurantNotifications(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(); 
        var query = new GetRestaurantNotificationsQuery(user);
        var notifications = await _mediator.Send(query, cancellationToken);

        return Ok(notifications);
    }

    [HttpDelete("{notificationId:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteNotification(Guid notificationId, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync();
        await _mediator.Send(new DeleteNotificationCommand(notificationId, user.Id), cancellationToken);
        return NoContent();
    }

    
    [HttpPost("send-invitation")]
    [Authorize(Roles = "RestaurantManager")]
    public async Task<ActionResult<Guid>> SendManagerInvitation([FromBody] RestaurantManagerInvitationDto dto, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUserAsync();

        var notificationId = await _mediator.Send(new SendRestaurantManagerInvitationCommand(
            dto.RestaurantId,
            currentUser,
            dto.RecipientId),
            cancellationToken);

        return Ok(notificationId);
    }

    [HttpPost("accept-invitation")]
    [Authorize]
    public async Task<IActionResult> AcceptManagerInvitation([FromBody] Guid NotificationId, CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUserAsync();

        await _mediator.Send(new AcceptRestaurantManagerInvitationCommand(
            NotificationId,
            currentUser),
            cancellationToken);

        return Ok("Invitation accepted successfully.");
    }
}
