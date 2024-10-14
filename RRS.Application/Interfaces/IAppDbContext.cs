using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RRS.Core.Models;

namespace RRS.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<RestaurantManagerData> RestaurantManagerDatas { get; set; }
    DbSet<Restaurant> Restaurants { get; set; }
    DbSet<Reservation> Reservations { get; set; }

    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
