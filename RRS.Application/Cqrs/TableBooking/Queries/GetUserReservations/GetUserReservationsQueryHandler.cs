using MediatR;
using Microsoft.AspNetCore.Identity;
using RRS.Application.Common.Exceptions;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Queries.GetUserReservations;

public class GetUserReservationsQueryHandler : IRequestHandler<GetUserReservationsQuery, List<Reservation>>
{
    private readonly UserManager<AppUser> _userManager;

    public GetUserReservationsQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<Reservation>> Handle(GetUserReservationsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
            throw new ReservationException("User not found");

        var reservations = request.Filter switch
        {
            ReservationFilter.Active => user.Reservations.Where(r => r.Status == ReservationStatus.Active).ToList(),
            ReservationFilter.All => user.Reservations.ToList(),
            _ => throw new ArgumentOutOfRangeException()
        };

        return reservations;
    }
}
