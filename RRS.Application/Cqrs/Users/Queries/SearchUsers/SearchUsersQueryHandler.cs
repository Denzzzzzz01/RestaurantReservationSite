using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Contracts.AppUser;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Users.Queries.SearchUsers;

public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, IEnumerable<AppUserDto>>
{
    private readonly UserManager<AppUser> _userManager;

    public SearchUsersQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<AppUserDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userManager.Users
            .Where(u => u.UserName.Contains(request.Query) || u.Email.Contains(request.Query))
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new AppUserDto { Id = u.Id, UserName = u.UserName})
            .ToListAsync(cancellationToken);
    }
}
