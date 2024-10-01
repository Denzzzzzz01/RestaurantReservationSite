using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RRS.Core.Models;

namespace RRS.Application.Interfaces;

public interface IAppDbContext
{

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
