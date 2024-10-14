using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Common.Exceptions;
using RRS.Application.Interfaces;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Queries.GetUserReservations;

public class GetUserReservationsQueryHandler : IRequestHandler<GetUserReservationsQuery, List<Reservation>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppDbContext _dbContext;

    public GetUserReservationsQueryHandler(UserManager<AppUser> userManager, IAppDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<List<Reservation>> Handle(GetUserReservationsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
            throw new ReservationException("User not found");

        var reservations = request.Filter switch
        {
            ReservationFilter.Active => await _dbContext.Reservations
                .Where(r => r.UserId == user.Id && r.Status == ReservationStatus.Active)
                .ToListAsync(cancellationToken),
            ReservationFilter.All => await _dbContext.Reservations
                .Where(r => r.UserId == user.Id)
                .ToListAsync(cancellationToken),
            _ => throw new ArgumentOutOfRangeException()
        };

        return reservations;
    }
}


