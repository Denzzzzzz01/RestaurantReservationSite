using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Common.Exceptions;
using RRS.Application.Cqrs.Notifications.Events.ReservationStatusChangedEvent;
using RRS.Application.Interfaces;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Commands.ChangeReservationStatus;

public class ChangeReservationStatusCommandHandler : IRequestHandler<ChangeReservationStatusCommand, Guid>
{
    private readonly IAppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;

    public ChangeReservationStatusCommandHandler(
        IAppDbContext dbContext,
        UserManager<AppUser> userManager,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(ChangeReservationStatusCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _dbContext.Reservations
            .Include(r => r.Restaurant)
            .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken)
            ?? throw new ReservationException("Reservation not found");

        var currentUser = await _userManager.Users
            .Include(u => u.RestaurantManagerData)
            .FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (currentUser?.RestaurantManagerData?.RestaurantId != reservation.RestaurantId)
            throw new ReservationException("You are not authorized to manage this restaurant.");

        reservation.Status = request.NewStatus;

        await _dbContext.SaveChangesAsync(cancellationToken);


        var reservationStatusChangedEvent = new ReservationStatusChangedEvent(
            reservation.Restaurant.Id,
            reservation.Restaurant.Name,
            reservation.User, 
            reservation.Status
        );
        await _mediator.Publish(reservationStatusChangedEvent, cancellationToken);

        return reservation.Id;
    }

}

