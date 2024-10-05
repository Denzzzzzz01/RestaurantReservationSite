using RRS.Core.Models;

namespace RRS.Application.Interfaces;

public interface ITokenService
{

    Task<string> CreateToken(AppUser appUser);
}
