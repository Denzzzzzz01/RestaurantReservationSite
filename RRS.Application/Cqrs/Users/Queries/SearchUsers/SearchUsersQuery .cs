using MediatR;
using RRS.Application.Contracts.AppUser;

namespace RRS.Application.Cqrs.Users.Queries.SearchUsers;

public record SearchUsersQuery(string Query, int Page, int PageSize) : IRequest<IEnumerable<AppUserDto>>;
