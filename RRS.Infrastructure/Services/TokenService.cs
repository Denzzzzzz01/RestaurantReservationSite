using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RRS.Application.Interfaces;
using RRS.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RRS.Infrastructure.Services;

public class TokenSerive : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;

    public TokenSerive(IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _configuration = configuration;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
        _userManager = userManager;
    }

    public async Task<string> CreateToken(AppUser appUser)
    {
        var roles = await _userManager.GetRolesAsync(appUser);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, appUser.UserName),
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescruiptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescruiptor);

        return tokenHandler.WriteToken(token);
    }
}