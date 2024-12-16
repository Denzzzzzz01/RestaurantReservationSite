using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Common.Configurations;
using RRS.Application.Common.Exceptions;
using RRS.Application.Cqrs.Notifications.Events.ReservationCreated;
using RRS.Application.Interfaces;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Commands.BookTable;

public class BookTableCommandHandler : IRequestHandler<BookTableCommand, Guid>
{
    private readonly IAppDbContext _dbContext;
    private readonly IReservationAvailabilityService _reservationAvailabilityService;
    private readonly IMediator _mediator;

    public BookTableCommandHandler(IAppDbContext dbContext, IReservationAvailabilityService reservationAvailabilityService, IMediator mediator)
    {
        _dbContext = dbContext;
        _reservationAvailabilityService = reservationAvailabilityService;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(BookTableCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            if (!await _reservationAvailabilityService.IsReservationTimeValidAsync(request.RestaurantId, request.StartTime, cancellationToken))
                throw new ReservationException("Reservation time must be within restaurant opening hours.");

            TimeSpan reservationDuration = TimeSpan.FromHours(ReservationSettings.BookingDurationInHours)
                .Add(TimeSpan.FromMinutes(ReservationSettings.BufferTimeInMinutes));
            var endTime = request.StartTime.Add(reservationDuration);

            var table = await _reservationAvailabilityService.GetAvailableTableAsync(
                request.RestaurantId,
                request.ReservationDate,
                request.StartTime,
                endTime,
                request.NumberOfSeats,
                cancellationToken
            );

            if (table is null)
                throw new ReservationException("No available tables for the requested number of seats.");

            _dbContext.Entry(request.User).State = EntityState.Unchanged;
                
            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                RestaurantId = request.RestaurantId,
                TableId = table.Id,  
                UserId = request.User.Id,
                User = request.User,
                ReservationDate = request.ReservationDate,
                StartTime = request.StartTime,
                EndTime = endTime,
                NumberOfSeats = request.NumberOfSeats,
                Status = ReservationStatus.Active
            };

            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var reservationCreatedEvent = new ReservationCreatedEvent(
            Restaurant: reservation.Restaurant,
            User: reservation.User,
            ReservationDate: reservation.ReservationDate,
            StartTime: reservation.StartTime,
            EndTime: reservation.EndTime
            );

            await _mediator.Publish(reservationCreatedEvent, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return reservation.Id;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
