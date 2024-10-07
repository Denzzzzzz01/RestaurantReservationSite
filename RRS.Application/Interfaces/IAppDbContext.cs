using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RRS.Core.Models;

namespace RRS.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<RestaurantManagerData> RestaurantManagerDatas { get; set; }
    DbSet<Restaurant> Restaurants { get; set; }

    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
