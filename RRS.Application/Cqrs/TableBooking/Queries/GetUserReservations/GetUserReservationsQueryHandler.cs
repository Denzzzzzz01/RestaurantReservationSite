using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Common.Exceptions;
using RRS.Application.Contracts.Reservations;
using RRS.Application.Interfaces;
using RRS.Core.Enums;
using RRS.Core.Models;

namespace RRS.Application.Cqrs.TableBooking.Queries.GetUserReservations;

public class GetUserReservationsQueryHandler : IRequestHandler<GetUserReservationsQuery, List<UserReservationDto>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppDbContext _dbContext;

    public GetUserReservationsQueryHandler(UserManager<AppUser> userManager, IAppDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<List<UserReservationDto>> Handle(GetUserReservationsQuery request, CancellationToken cancellationToken)
    {
        var reservations = request.Filter switch
        {
            ReservationFilter.Active => await _dbContext.Reservations
                .AsNoTracking()
                .Where(r => r.UserId == request.User.Id && r.Status == ReservationStatus.Active)
                .Include(r => r.Restaurant)
                .ToListAsync(cancellationToken),
            ReservationFilter.All => await _dbContext.Reservations
                .AsNoTracking() 
                .Where(r => r.UserId == request.User.Id)
                .Include(r => r.Restaurant)
                .ToListAsync(cancellationToken),
            _ => throw new ArgumentOutOfRangeException()
        };

        var reservationDtos = reservations.Select(r => 
            r.Adapt<UserReservationDto>()
        ).ToList();

        return reservationDtos;
    }
}


