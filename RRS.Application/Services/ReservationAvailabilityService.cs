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

    public async Task<(bool, Restaurant)> HasAvailableSeatsAsync(Guid restaurantId, DateTime reservationDate, TimeSpan startTime, TimeSpan endTime, int requestedSeats, CancellationToken cancellationToken)
    {
        var restaurant = await _dbContext.Restaurants
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(r => r.Id == restaurantId, cancellationToken)
            ?? throw new ReservationException("Restaurant not found");

        var overlappingReservations = await _dbContext.Reservations
            .Where(r => r.RestaurantId == restaurant.Id &&
                        r.ReservationDate.Date == reservationDate.Date &&
                        r.StartTime < endTime &&
                        r.EndTime > startTime)
            .SumAsync(r => r.NumberOfSeats, cancellationToken);

        bool hasSeats = overlappingReservations + requestedSeats <= restaurant.SeatingCapacity;
        return (hasSeats, restaurant);
    }
}
