using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRS.Application.Contracts.Reservations;
using RRS.Application.Cqrs.TableBooking.Commands.BookTable;
using RRS.Application.Cqrs.TableBooking.Commands.ChangeReservationStatus;
using RRS.Application.Cqrs.TableBooking.Queries.GetRestaurantReservation;
using RRS.Application.Cqrs.TableBooking.Queries.GetUserReservations;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Api.Controllers;

[Authorize]
public class ReservationsController : BaseController
{
    private readonly IMediator _mediator;

    public ReservationsController(UserManager<AppUser> userManager, IMediator mediator) : base(userManager)
    {
        _mediator = mediator;
    }

    [HttpGet("restaurant/{restaurantId}")]
    public async Task<IActionResult> GetRestaurantReservations(Guid restaurantId, [FromQuery] ReservationFilter filter = ReservationFilter.All)
    {
        var reservations = await _mediator.Send(new GetRestaurantReservationsQuery(restaurantId, filter));
        return Ok(reservations);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserReservations([FromQuery] ReservationFilter filter = ReservationFilter.All)
    {
        var user = await GetCurrentUserAsync();
        var reservations = await _mediator.Send(new GetUserReservationsQuery(user, filter));
        return Ok(reservations);
    }

    [HttpPost("book-table")]
    public async Task<IActionResult> BookTable([FromBody] BookTableDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        

        var user = await GetCurrentUserAsync();
        var command = dto.Adapt<BookTableCommand>() with { User = user };
        //command = command with { User = user };

        var reservationId = await _mediator.Send(command);
        return Ok(reservationId);
    }

    [HttpPost("change-status")]
    public async Task<IActionResult> ChangeReservationStatus(Guid reservationId, ReservationStatus newStatus)
    {
        var user = await GetCurrentUserAsync();

        var command = new ChangeReservationStatusCommand(reservationId, newStatus, user.Id);
        reservationId = await _mediator.Send(command);

        return Ok(reservationId);
    }

}
