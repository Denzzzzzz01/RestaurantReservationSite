using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRS.Application.Cqrs.Users.Queries.SearchUsers;
using RRS.Core.Models;

namespace RRS.Api.Controllers;

public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(UserManager<AppUser> userManager, IMediator mediator) : base(userManager)
    {
        _mediator = mediator;
    }


    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query must not be empty.");

        var users = await _mediator.Send(new SearchUsersQuery(query, page, pageSize));
        return Ok(users);
    }
}
