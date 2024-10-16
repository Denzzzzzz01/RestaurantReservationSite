using Microsoft.AspNetCore.Identity;

namespace RRS.Infrastructure.Persistence;

public class DataInitializer
{
    private readonly AppDbContext _context;

    public DataInitializer(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (_context.Roles.Any())
        {
            _context.Roles.RemoveRange(_context.Roles);
            _context.SaveChanges();
        }

        var roles = new List<IdentityRole<Guid>>
        {
            new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "User",
                NormalizedName = "USER"
            }
        };

        _context.Roles.AddRange(roles);
        _context.SaveChanges();
    }
}
