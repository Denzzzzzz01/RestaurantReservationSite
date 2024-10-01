using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRS.Core.Models;
using System.Security.Claims;

namespace RRS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected readonly UserManager<AppUser> _userManager;

    public BaseController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    protected async Task<AppUser> GetCurrentUserAsync()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser is null)
            throw new UnauthorizedAccessException("User not found");

        return appUser;
    }
}
