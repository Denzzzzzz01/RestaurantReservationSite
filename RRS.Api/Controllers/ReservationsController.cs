using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRS.Application.Contracts.Reservations;
using RRS.Application.Cqrs.TableBooking.Commands.BookTable;
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
        var reservations = await _mediator.Send(new GetUserReservationsQuery(user.Id, filter));
        return Ok(reservations);
    }

    [HttpPost(nameof(BookTable))]
    public async Task<IActionResult> BookTable([FromBody] BookTableDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        

        var user = await GetCurrentUserAsync();
        var command = dto.Adapt<BookTableCommand>();
        command = command with { UserId = user.Id };

        var reservationId = await _mediator.Send(command);
        return Ok(reservationId);
    }
}
