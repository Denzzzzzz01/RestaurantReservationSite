using MediatR;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.Table.Commands.AddRestaurantTables;

public record AddRestaurantTablesCommand(Guid RestaurantId, AppUser User, int NumberOfTables, int TableCapacity, string? Description) : IRequest<Unit>;
