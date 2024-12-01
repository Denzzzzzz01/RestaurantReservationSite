using Microsoft.AspNetCore.Identity;
using RRS.Core.Models;

namespace RRS.Infrastructure.Services;


public class DataInitializer
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public DataInitializer(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedRolesAndUsersAsync()
    {
        var adminRole = new IdentityRole<Guid> 
        { 
            Id = Guid.Parse("2da6335d-c40d-4a34-9573-4c8f056f6bb8"),
            Name = "Admin", 
            NormalizedName = "ADMIN" 
        };
        var userRole = new IdentityRole<Guid>
        {
            Id = Guid.Parse("ea5059a5-dd06-4feb-a355-ba2018acbd24"),
            Name = "User", 
            NormalizedName = "USER" 
        };
        var managerRole = new IdentityRole<Guid>
        {
            Id = Guid.Parse("5E226760-1A39-4ACB-B6FB-DA1A7E2D10A5"),
            Name = "RestaurantManager",
            NormalizedName = "RESTAURANTMANAGER"
        };

        if (!await _roleManager.RoleExistsAsync(adminRole.Name))
        {
            await _roleManager.CreateAsync(adminRole);
        }

        if (!await _roleManager.RoleExistsAsync(userRole.Name))
        {
            await _roleManager.CreateAsync(userRole);
        }

        if (!await _roleManager.RoleExistsAsync(managerRole.Name))
        {
            await _roleManager.CreateAsync(managerRole);
        }

        var adminUser = await _userManager.FindByNameAsync("admin");
        if (adminUser is null)
        {
            adminUser = new AppUser
            {
                UserName = "admin",
                Email = "admin@mail.com",
            };

            var createResult = await _userManager.CreateAsync(adminUser, "admin");
            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, adminRole.Name);
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }
        }
    }
}
