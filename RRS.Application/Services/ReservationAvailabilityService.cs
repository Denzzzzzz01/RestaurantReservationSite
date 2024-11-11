using Microsoft.EntityFrameworkCore;
using RRS.Application.Common.Configurations;
using RRS.Application.Common.Exceptions;
using RRS.Application.Interfaces;
using RRS.Core.Models;

namespace RRS.Application.Services;

public class ReservationAvailabilityService : IReservationAvailabilityService
{
    private readonly IAppDbContext _dbContext;

    public ReservationAvailabilityService(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsReservationTimeValidAsync(Guid restaurantId, TimeSpan startTime, CancellationToken cancellationToken)
    {
        var restaurant = await _dbContext.Restaurants
            .FirstOrDefaultAsync(r => r.Id == restaurantId, cancellationToken)
            ?? throw new ReservationException("Restaurant not found");

        TimeSpan reservationDuration = TimeSpan.FromHours(ReservationSettings.BookingDurationInHours)
            .Add(TimeSpan.FromMinutes(ReservationSettings.BufferTimeInMinutes));
        var endTime = startTime.Add(reservationDuration);

        return startTime >= restaurant.OpeningHour && endTime <= restaurant.ClosingHour;
    }

    public async Task<RestaurantTable> GetAvailableTableAsync(Guid restaurantId, DateTime reservationDate, TimeSpan startTime, TimeSpan endTime, int requestedSeats, CancellationToken cancellationToken)
    {
        var availableTables = await _dbContext.RestaurantTables
            .Where(t => t.RestaurantId == restaurantId && t.Capacity >= requestedSeats && t.IsAvailable)
            .OrderBy(t => t.Capacity)  
            .ToListAsync(cancellationToken);

        foreach (var table in availableTables)
        {
            var overlappingReservations = await _dbContext.Reservations
                .Where(r => r.RestaurantId == restaurantId &&
                            r.TableId == table.Id &&
                            r.ReservationDate.Date == reservationDate.Date &&
                            r.StartTime < endTime &&
                            r.EndTime > startTime)
                .ToListAsync(cancellationToken);

            if (!overlappingReservations.Any())
            {
                return table;
            }
        }

        throw new ReservationException("No available tables found for the requested number of seats.");
    }
}

