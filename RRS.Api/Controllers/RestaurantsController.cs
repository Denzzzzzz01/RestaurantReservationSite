using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRS.Api.Interfaces;
using RRS.Application.Contracts.Common;
using RRS.Application.Contracts.Restaurant;
using RRS.Application.Cqrs.Restaurant.Commands.AddRestaurant;
using RRS.Application.Cqrs.Restaurant.Commands.DeleteRestaurant;
using RRS.Application.Cqrs.Restaurant.Commands.UpdateRestaurant;
using RRS.Application.Cqrs.Restaurant.Queries.GetRestaurantDetailsQuery;
using RRS.Application.Cqrs.Restaurant.Queries.GetRestaurants;
using RRS.Application.Cqrs.Restaurant.Queries.SearchRestaurants;
using RRS.Application.Cqrs.Restaurants.Commands.UploadRestaurantLogo;
using RRS.Core.Models;

namespace RRS.Api.Controllers;

public class RestaurantsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IFileStorageService _fileStorageService;
    public RestaurantsController(UserManager<AppUser> userManager, IMediator mediator, IFileStorageService fileStorageService) : base(userManager)
    {
        _mediator = mediator;
        _fileStorageService = fileStorageService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddRestaurant([FromBody] AddRestaurantDto dto, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync();

        var command = new AddRestaurantCommand(
            user,
            dto.Name,
            dto.Address,
            TimeSpan.Parse(dto.OpeningHour),
            TimeSpan.Parse(dto.ClosingHour),
            dto.PhoneNumber,
            dto.Website
        );

        var addedRestaurantId = await _mediator.Send(command, cancellationToken);

        return Ok(addedRestaurantId);
    }

    [Authorize(Roles = "RestaurantManager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRestaurant(Guid id, [FromBody] UpdateRestaurantDto dto, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync();

        var command = new UpdateRestaurantCommand(
            user,
            id,
            dto.Name,
            dto.Address,
            TimeSpan.Parse(dto.OpeningHour),
            TimeSpan.Parse(dto.ClosingHour),
            dto.PhoneNumber,
            dto.Website
        );

        var updatedRestaurantId = await _mediator.Send(command, cancellationToken);

        return Ok(updatedRestaurantId);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRestaurant(Guid id, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync();

        var command = new DeleteRestaurantCommand(id, user);
        var deletedRestaurantId = await _mediator.Send(command, cancellationToken);

        return Ok(deletedRestaurantId);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<RestaurantSummaryDto>>> GetAllRestaurants(int pageNumber = 1, int pageSize = 10)
    {
        var query = new GetRestaurantsQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);

        if (result.Items == null || !result.Items.Any())
        {
            return NotFound("No restaurants found.");
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto>> GetRestaurant(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid restaurant ID.");
        }

        var query = new GetRestaurantDetailsQuery(id);
        var restaurant = await _mediator.Send(query, cancellationToken);

        if (restaurant is null)
        {
            return NotFound("Restaurant not found.");
        }

        return Ok(restaurant);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedResult<RestaurantSummaryDto>>> SearchRestaurants([FromQuery] SearchRestaurantsDto searchDto, int pageNumber = 1, int pageSize = 10)
    {
        var query = new SearchRestaurantsQuery(searchDto, pageNumber, pageSize);
        var restaurants = await _mediator.Send(query);

        if (restaurants is null || !restaurants.Items.Any())
            return NotFound("No restaurants found.");
        

        return Ok(restaurants);
    }

    [HttpPost("upload-logo/{restaurantId}")]
    public async Task<IActionResult> UploadLogo(Guid restaurantId, IFormFile file, CancellationToken cancellationToken)
    {
        try
        {
            if (!file.ContentType.StartsWith("image/"))
                throw new InvalidOperationException("Invalid file type.");

            var logoUrl = await _fileStorageService.SaveFileAsync(restaurantId, file, "restaurants", cancellationToken);
            await _mediator.Send(new UpdateRestaurantLogoCommand(restaurantId, logoUrl), cancellationToken);

            return Ok(new { logoUrl });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

}


