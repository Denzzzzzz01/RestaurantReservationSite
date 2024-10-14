using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRS.Core.Models;

namespace RRS.Infrastructure.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasMany(u => u.Reservations)
            .WithOne(reserv => reserv.User)
            .HasForeignKey(reserv => reserv.UserId);
    }
}