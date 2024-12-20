using MediatR;
using RRS.Application.Contracts.Table;

namespace RRS.Application.Cqrs.Table.Queries.GetRestaurantTables;

public record GetRestaurantTablesQuery(Guid RestaurantId) : IRequest<List<RestaurantTableDto>>;

