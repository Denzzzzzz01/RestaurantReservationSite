using RRS.Core.Models;

namespace RRS.Application.Interfaces;

public interface ITokenService
{

    string CreateToken(AppUser appUser);
}
