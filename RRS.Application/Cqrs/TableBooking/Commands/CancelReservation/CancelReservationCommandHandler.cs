using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Common.Exceptions;
using RRS.Application.Cqrs.Notifications.Events.ReservationCancelled;
using RRS.Application.Interfaces;
using RRS.Core.Enums;

namespace RRS.Application.Cqrs.TableBooking.Commands.CancelReservation;

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, Unit>
{
    private readonly IAppDbContext _dbContext;
    private readonly IMediator _mediator;

    public CancelReservationCommandHandler(IAppDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _dbContext.Reservations
            .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken)
            ?? throw new ReservationException("Reservation not found.");

        if (reservation.UserId != request.UserId)
            throw new ReservationException("You are not authorized to cancel this reservation.");

        if (reservation.Status == ReservationStatus.Cancelled)
            throw new ReservationException("Reservation is already cancelled.");

        reservation.Status = ReservationStatus.Cancelled;

        await _dbContext.SaveChangesAsync(cancellationToken);


        var reservationCancelledEvent = new ReservationCancelledEvent(
        reservation.Restaurant,
        reservation.User,
        ReservationStatus.Cancelled
        );
        await _mediator.Publish(reservationCancelledEvent, cancellationToken);

        return Unit.Value;
    }
}
