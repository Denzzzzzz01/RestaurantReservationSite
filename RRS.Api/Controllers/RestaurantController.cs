using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRS.Application.Contracts.Restaurant;
using RRS.Application.Cqrs.Restaurant.Commands.AddRestaurant;
using RRS.Application.Cqrs.Restaurant.Commands.DeleteRestaurant;
using RRS.Application.Cqrs.Restaurant.Commands.UpdateRestaurant;
using RRS.Application.Cqrs.Restaurant.Queries.GetRestaurants;
using RRS.Application.Interfaces;
using RRS.Core.Models;

namespace RRS.Api.Controllers;

public class RestaurantController : BaseController
{
    private readonly IAppDbContext _dbContext;
    private readonly IMediator _mediator;
    public RestaurantController(UserManager<AppUser> userManager, IAppDbContext dbContext, IMediator mediator) : base(userManager)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost(nameof(AddRestaurant))]
    public async Task<IActionResult> AddRestaurant([FromBody] AddRestaurantDto dto)
    {
        var user = await GetCurrentUserAsync();

        if (user is null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var command = new AddRestaurantCommand(
            user,
            dto.Name,
           dto.Address,
            dto.SeatingCapacity,
            TimeSpan.Parse(dto.OpeningHour),
            TimeSpan.Parse(dto.ClosingHour),
            dto.PhoneNumber,
            dto.Website
        );

        var restaurantId = await _mediator.Send(command);

        return Ok(restaurantId);
    }

    [Authorize]
    [HttpPut(nameof(UpdateRestaurant))]
    public async Task<IActionResult> UpdateRestaurant([FromBody] UpdateRestaurantDto dto)
    {
        var user = await GetCurrentUserAsync();

        if (user is null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var command = new UpdateRestaurantCommand(
            user,
            dto.Id,
            dto.Name,
            dto.Address,
            dto.SeatingCapacity,
            TimeSpan.Parse(dto.OpeningHour),
            TimeSpan.Parse(dto.ClosingHour),
            dto.PhoneNumber,
            dto.Website
        );

        var updatedRestaurantId = await _mediator.Send(command);

        return Ok(updatedRestaurantId);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete(nameof(DeleteRestaurant))]
    public async Task<IActionResult> DeleteRestaurant(Guid id)
    {
        var user = await GetCurrentUserAsync();

        if (user is null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var command = new DeleteRestaurantCommand(id, user);
        var deletedRestaurantId = await _mediator.Send(command);

        return Ok(deletedRestaurantId);
    }

    [HttpGet("GetAllRestaurants")]
    public async Task<IActionResult> GetAllRestaurants(int PageNumber = 1, int PageSize = 10)
    {

        var query = new GetRestaurantsQuery(PageNumber, PageSize);
        var restaurants = await _mediator.Send(query);

        if (restaurants == null || !restaurants.Any())
        {
            return NotFound("No restaurants found.");
        }

        return Ok(restaurants);
    }
}
