using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RRS.Application.Interfaces;
using RRS.Core.Models;
using RRS.Infrastructure.Persistence.Configurations;

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
        builder.ApplyConfiguration(new ReservationConfiguration());

        var roles = new List<IdentityRole<Guid>>
        {
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("2da6335d-c40d-4a34-9573-4c8f056f6bb8"),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("ea5059a5-dd06-4feb-a355-ba2018acbd24"),
                Name = "User",
                NormalizedName = "USER"
            }
        };

        builder.Entity<IdentityRole<Guid>>().HasData(roles);

        //CreateAdminUserAsync().Wait();
    }

    //private async Task CreateAdminUserAsync()
    //{
    //    var adminUser = new AppUser
    //    {
    //        UserName = "admin",
    //        Email = "admin@mail.com",
    //        NormalizedUserName = "ADMIN",
    //        NormalizedEmail = "ADMIN@MAIL.COM"
    //    };

    //    var password = "admin";
    //    var hashedPassword = new PasswordHasher<AppUser>().HashPassword(adminUser, password);

    //    adminUser.PasswordHash = hashedPassword;

    //    var role = await _roleManager.FindByNameAsync("Admin");
    //    if (role != null)
    //    {
    //        // Сначала создаем пользователя, а затем добавляем роль
    //        var result = await _userManager.CreateAsync(adminUser);

    //        if (result.Succeeded)
    //        {
    //            await _userManager.AddToRoleAsync(adminUser, "Admin");
    //        }
    //    }
    //}

    public DbSet<RestaurantManagerData> RestaurantManagerDatas { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<RestaurantTable> RestaurantTables { get; set; }
}
