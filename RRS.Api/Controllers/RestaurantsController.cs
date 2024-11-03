using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRS.Application.Contracts.Restaurant;
using RRS.Application.Cqrs.Restaurant.Commands.AddRestaurant;
using RRS.Application.Cqrs.Restaurant.Commands.DeleteRestaurant;
using RRS.Application.Cqrs.Restaurant.Commands.UpdateRestaurant;
using RRS.Application.Cqrs.Restaurant.Queries.GetRestaurantDetailsQuery;
using RRS.Application.Cqrs.Restaurant.Queries.GetRestaurants;
using RRS.Application.Cqrs.Restaurant.Queries.SearchRestaurants;
using RRS.Core.Models;

namespace RRS.Api.Controllers;

public class RestaurantsController : BaseController
{
    private readonly IMediator _mediator;
    public RestaurantsController(UserManager<AppUser> userManager, IMediator mediator) : base(userManager)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
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

        var addedRestaurantId = await _mediator.Send(command);

        return Ok(addedRestaurantId);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRestaurant(Guid id, [FromBody] UpdateRestaurantDto dto)
    {
        var user = await GetCurrentUserAsync();

        if (user is null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var command = new UpdateRestaurantCommand(
            user,
            id,
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
    [HttpDelete("{id}")]
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

    [HttpGet]
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRestaurant(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid restaurant ID.");
        }

        var query = new GetRestaurantDetailsQuery(id);
        var restaurant = await _mediator.Send(query);

        if (restaurant is null)
        {
            return NotFound("Restaurant not found.");
        }

        return Ok(restaurant);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchRestaurants([FromQuery] SearchRestaurantsDto searchDto)
    {
        var query = new SearchRestaurantsQuery(searchDto);
        var restaurants = await _mediator.Send(query);

        if (restaurants is null || !restaurants.Any())
            return NotFound("No restaurants found.");
        

        return Ok(restaurants);
    }
}


