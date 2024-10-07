using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RRS.Core.Models;

namespace RRS.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<RestaurantManagerData> RestaurantManagerDatas { get; set; }
    DbSet<Restaurant> Restaurants { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
