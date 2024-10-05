using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Interfaces;
using RRS.Core.Models;

namespace RRS.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new AppUserConfiguration());
        builder.ApplyConfiguration(new RestaurantConfiguration());
        builder.ApplyConfiguration(new RestaurantManagerDataConfiguration());

        var roles = new List<IdentityRole<Guid>>
        {
            new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name= "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name= "User",
                NormalizedName = "USER"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name= "RestaurantManager",
                NormalizedName = "RESTAURANTMANAGER"
            }
        };
        builder.Entity<IdentityRole<Guid>>().HasData(roles);
    }

    public DbSet<RestaurantManagerData> RestaurantManagerDatas { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
}
