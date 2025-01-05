using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Contracts.Account;
using RRS.Application.Interfaces;
using RRS.Core.Models;
using System.Security.Claims;

namespace RRS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : BaseController
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AccountController> _logger;
    //private readonly ICacheService _cache;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, ILogger<AccountController> logger/*, ICacheService cache*/) : base(userManager)
    {
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = logger;
        //_cache = cache;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (registerDto.Password != registerDto.ConfirmPassword)
                return BadRequest("Password and Confirm Password do not match.");
           

            //var cacheKey = $"User_{registerDto.Email}";
            //var existingUser = await _cache.GetAsync<AppUser>(cacheKey);
            //if (existingUser != null)
            //{
            //    _logger.LogInformation("User {Email} already exists in cache", registerDto.Email);
            //    return BadRequest("User already exists");
            //}

            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };
            var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (roleResult.Succeeded)
                {
                    var response = new UserResponseDto
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Token = await _tokenService.CreateToken(user)
                    };
                    _logger.LogInformation("User registered: {Username}, {Email}", user.UserName, user.Email);
                    //await _cache.SetAsync(cacheKey, user, new DistributedCacheEntryOptions
                    //{
                    //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    //}, ct);
                    return Ok(response);
                }
                else
                {
                    _logger.LogError("Error occurred while adding role to user {Username}: {Error}", user.UserName, roleResult.Errors);
                    return StatusCode(500, roleResult.Errors);
                }
            }
            else
            {
                _logger.LogError("Error occurred while creating user: {Username}, {Email}", user.UserName, user.Email);
                return StatusCode(500, createdUser.Errors);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while registering user");
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower(), cancellationToken);
        if (user is null)
            return Unauthorized("Invalid Email!");


        //var cacheKey = $"User_{loginDto.Email}";  
        //var existingUser = await _cache.GetAsync<AppUser>(cacheKey);  
        //if (existingUser != null && existingUser.Email.ToLower() == loginDto.Email.ToLower())
        //{
            //_logger.LogInformation("User {Email} found in cache", loginDto.Email);
            //if (!string.IsNullOrEmpty(loginDto.Password))
            //{
            //    var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            //    if (result.Succeeded)
            //    {
            //        var response = new UserResponseDto
            //        {
            //            Username = user.UserName,
            //            Email = user.Email,
            //            Token =  await _tokenService.CreateToken(user)
            //        };
            //        _logger.LogInformation("User logged in: {Username}, {Email}", user.UserName, user.Email);
            //        return Ok(response);
            //    }
            //    else
            //    {
            //        _logger.LogError("Incorrect password for user {Username}, {Email}", user.UserName, user.Email);
            //        return Unauthorized("Incorrect password");
            //    }
            //}
            //else
            //{
            //    _logger.LogError("Password not provided for user {Username}, {Email}", user.UserName, user.Email);
            //    return Unauthorized("Password not provided");
            //}
        //}

        var resultDb = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
        if (!resultDb.Succeeded)
            return Unauthorized("User not found and/or incorrect password");

        var responseDb = new UserResponseDto
        {
            Username = user.UserName,
            Email = user.Email,
            Token = await _tokenService.CreateToken(user)
        };
        _logger.LogInformation("User logged in: {Username}, {Email}", user.UserName, user.Email);
        //await _cache.SetAsync(cacheKey, user, new DistributedCacheEntryOptions
        //{
        //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        //}, ct);
        return Ok(responseDb);
    }


    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        var appUser = await GetCurrentUserAsync();

        return Ok(new
        {
            Roles = roles
        });
    }

    [Authorize]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var appUser = await GetCurrentUserAsync();
        var newToken = await _tokenService.CreateToken(appUser);
        return Ok(new { Token = newToken });
    }

    [HttpGet("restaurant")]
    [Authorize(Roles = "RestaurantManager")]
    public async Task<IActionResult> GetUserRestaurant(CancellationToken cancellationToken)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var appUser = await _userManager.Users
            .Include(u => u.RestaurantManagerData)
            .ThenInclude(rmd => rmd.Restaurant) 
            .FirstOrDefaultAsync(u => u.Email == userEmail, cancellationToken);

        if (appUser is null)
            throw new UnauthorizedAccessException("User not found");

        return Ok(new { id = appUser.RestaurantManagerData.Restaurant.Id, name = appUser.RestaurantManagerData.Restaurant.Name });
    }

}
