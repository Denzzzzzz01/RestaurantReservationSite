using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RRS.Application.Interfaces;

public interface IAppDbContext
{

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
