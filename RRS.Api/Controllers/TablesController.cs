using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRS.Application.Contracts.Table;
using RRS.Application.Cqrs.Table.Commands.AddRestaurantTables;
using RRS.Application.Cqrs.Table.Queries.GetRestaurantTables;
using RRS.Core.Models;
using System.Threading;

namespace RRS.Api.Controllers;

public class TablesController : BaseController
{
    private readonly IMediator _mediator;
    public TablesController(UserManager<AppUser> userManager, IMediator mediator) : base(userManager)
    {
        _mediator = mediator;
    }
    
    [HttpGet("restaurants/{restaurantId:guid}/tables")]
    public async Task<IActionResult> GetRestaurantTables(Guid restaurantId, CancellationToken cancellationToken)
    {
        var query = new GetRestaurantTablesQuery(restaurantId);
        var tables = await _mediator.Send(query, cancellationToken);

        if (tables == null || !tables.Any())
            return NotFound("No tables found for the restaurant.");

        return Ok(tables);
    }

    [Authorize(Roles = "RestaurantManager")]
    [HttpPost("{restaurantId:guid}/tables")]
    public async Task<IActionResult> AddRestaurantTables(Guid restaurantId, [FromBody] AddRestaurantTablesDto dto, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync();

        if (user is null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var command = new AddRestaurantTablesCommand(
             restaurantId,
             user,
            dto.NumberOfTables,
            dto.TableCapacity,
            dto.Description
        );

        await _mediator.Send(command, cancellationToken);

        return Ok($"Successfully added {dto.NumberOfTables} tables to the restaurant.");
    }


}
