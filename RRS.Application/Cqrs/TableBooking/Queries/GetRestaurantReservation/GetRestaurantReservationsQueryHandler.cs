using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Common.Exceptions;
using RRS.Application.Contracts.Reservations;
using RRS.Application.Interfaces;
using RRS.Core.Enums;

namespace RRS.Application.Cqrs.TableBooking.Queries.GetRestaurantReservation;

public class GetRestaurantReservationsQueryHandler : IRequestHandler<GetRestaurantReservationsQuery, List<RestaurantReservationDto>>
{
    private readonly IAppDbContext _dbContext;

    public GetRestaurantReservationsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RestaurantReservationDto>> Handle(GetRestaurantReservationsQuery request, CancellationToken cancellationToken)
    {
        var restaurant = await _dbContext.Restaurants
            .AsNoTracking()
            .Include(r => r.Reservations)
                .ThenInclude(r => r.User) 
            .FirstOrDefaultAsync(r => r.Id == request.RestaurantId, cancellationToken);

        if (restaurant == null)
            throw new ReservationException("Restaurant not found");

        var reservations = request.Filter switch
        {
            ReservationFilter.Active => restaurant.Reservations.Where(r => r.Status == ReservationStatus.Active).ToList(),
            ReservationFilter.All => restaurant.Reservations.ToList(),
            _ => throw new ArgumentOutOfRangeException()
        };

        var reservationDtos = reservations.Select(r => 
            r.Adapt<RestaurantReservationDto>()
        ).ToList();

        return reservationDtos;
    }
}


