using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRS.Application.Contracts.Restaurant;
using RRS.Application.Cqrs.Restaurant.Commands.AddRestaurantCommand;
using RRS.Application.Cqrs.Restaurant.Query.GetRestaurants;
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

        if (user == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var command = new AddRestaurantCommand(
            user,
            dto.Name,
            new Address
            {
                Street = dto.Address.Street,
                City = dto.Address.City,
                Country = dto.Address.Country,
                State = dto.Address.State,
                Latitude = dto.Address.Latitude,
                Longitude = dto.Address.Longitude
            },
            dto.SeatingCapacity,
            dto.OpeningHour,
            dto.ClosingHour,
            dto.PhoneNumber,
            dto.Website
        );

        var restaurantId = await _mediator.Send(command);

        return Ok(restaurantId);
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
