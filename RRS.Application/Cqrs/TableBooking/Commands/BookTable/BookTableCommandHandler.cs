using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Common.Configurations;
using RRS.Application.Common.Exceptions;
using RRS.Application.Interfaces;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Commands.BookTable;

public class BookTableCommandHandler : IRequestHandler<BookTableCommand, Guid>
{
    private readonly ReservationSettings _reservationSettings;
    private readonly IAppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public BookTableCommandHandler(IAppDbContext dbContext, UserManager<AppUser> userManager, ReservationSettings reservationSettings)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _reservationSettings = reservationSettings;
    }

    public async Task<Guid> Handle(BookTableCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _dbContext.Restaurants
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(r => r.Id == request.RestaurantId, cancellationToken);
        if (restaurant is null)
            throw new ReservationException("Restaurant not found");

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new ReservationException("User not found");

        TimeSpan reservationDuration = TimeSpan.FromHours(_reservationSettings.BookingDurationInHours)
            .Add(TimeSpan.FromMinutes(_reservationSettings.BufferTimeInMinutes));
        var endTime = request.StartTime.Add(reservationDuration);
        if (request.StartTime > restaurant.ClosingHour.Subtract(TimeSpan.FromHours(_reservationSettings.BookingDurationInHours)))
            throw new ReservationException($"Reservation time must be at least {_reservationSettings.BookingDurationInHours} hours before closing.");
        

        var overlappingReservations = restaurant.Reservations
            .Where(r => r.ReservationDate.Date == request.ReservationDate.Date &&
                r.StartTime < endTime &&
                r.EndTime > request.StartTime)
            .Sum(r => r.NumberOfSeats);
        if (overlappingReservations + request.NumberOfSeats > restaurant.SeatingCapacity)
            throw new ReservationException("Not enough available seats for the requested time.");
        

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RestaurantId = request.RestaurantId,
            UserId = request.UserId,
            ReservationDate = request.ReservationDate,
            StartTime = request.StartTime,
            EndTime = endTime,
            NumberOfSeats = request.NumberOfSeats,
            Status = ReservationStatus.Active,
        };

        _dbContext.Reservations.Add(reservation);

        restaurant.Reservations.Add(reservation);
        user.Reservations.Add(reservation);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}

