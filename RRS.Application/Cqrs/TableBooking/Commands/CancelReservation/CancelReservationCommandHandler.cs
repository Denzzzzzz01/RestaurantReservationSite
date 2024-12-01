using MediatR;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Common.Exceptions;
using RRS.Application.Interfaces;
using RRS.Core.Enums;

namespace RRS.Application.Cqrs.TableBooking.Commands.CancelReservation;

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, Unit>
{
    private readonly IAppDbContext _dbContext;

    public CancelReservationCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
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

        return Unit.Value;
    }
}
