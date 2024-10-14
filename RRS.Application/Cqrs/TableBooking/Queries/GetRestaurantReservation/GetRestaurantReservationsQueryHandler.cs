﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Common.Exceptions;
using RRS.Application.Interfaces;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Queries.GetRestaurantReservation;

public class GetRestaurantReservationsQueryHandler : IRequestHandler<GetRestaurantReservationsQuery, List<Reservation>>
{
    private readonly IAppDbContext _dbContext;

    public GetRestaurantReservationsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Reservation>> Handle(GetRestaurantReservationsQuery request, CancellationToken cancellationToken)
    {
        var restaurant = await _dbContext.Restaurants
            .AsNoTracking()
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(r => r.Id == request.RestaurantId, cancellationToken);

        if (restaurant == null)
            throw new ReservationException("Restaurant not found");

        var reservations = request.Filter switch
        {
            ReservationFilter.Active => restaurant.Reservations.Where(r => r.Status == ReservationStatus.Active).ToList(),
            ReservationFilter.All => restaurant.Reservations.ToList(),
            _ => throw new ArgumentOutOfRangeException()
        };

        return reservations;
    }
}
