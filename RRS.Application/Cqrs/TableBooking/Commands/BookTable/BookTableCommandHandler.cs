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
    private readonly IAppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public BookTableCommandHandler(IAppDbContext dbContext, UserManager<AppUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
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

        TimeSpan reservationDuration = TimeSpan.FromHours(ReservationSettings.BookingDurationInHours)
            .Add(TimeSpan.FromMinutes(ReservationSettings.BufferTimeInMinutes));
        var endTime = request.StartTime.Add(reservationDuration);

        if (request.StartTime < restaurant.OpeningHour ||
            request.StartTime > restaurant.ClosingHour.Subtract(reservationDuration))
        {
            throw new ReservationException("Reservation time must be within restaurant opening hours.");
        }

        var overlappingReservations = restaurant.Reservations
            .Where(r => r.ReservationDate.Date == request.ReservationDate.Date &&
                r.StartTime < endTime &&
                r.EndTime > request.StartTime)
            .Sum(r => r.NumberOfSeats);

        if (overlappingReservations + request.NumberOfSeats > restaurant.SeatingCapacity)
        {
            throw new ReservationException("Not enough available seats for the requested time.");
        }

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RestaurantId = request.RestaurantId,
            Restaurant = restaurant,
            UserId = request.UserId,
            User = user,
            ReservationDate = request.ReservationDate,
            StartTime = request.StartTime,
            EndTime = endTime,
            NumberOfSeats = request.NumberOfSeats,
            Status = ReservationStatus.Active,
        };

        _dbContext.Reservations.Add(reservation);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}


