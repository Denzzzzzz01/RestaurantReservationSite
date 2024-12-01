using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Common.Exceptions;
using RRS.Application.Interfaces;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Commands.ChangeReservationStatus;

public class ChangeReservationStatusCommandHandler : IRequestHandler<ChangeReservationStatusCommand, Guid>
{
    private readonly IAppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public ChangeReservationStatusCommandHandler(IAppDbContext dbContext, UserManager<AppUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
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

        return reservation.Id;
    }

}

