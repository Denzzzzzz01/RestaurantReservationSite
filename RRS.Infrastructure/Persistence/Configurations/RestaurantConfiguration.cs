using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RRS.Core.Models;

namespace RRS.Infrastructure.Persistence.Configurations;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(r => r.Manageres)
            .WithOne(m => m.Restaurant)
            .HasForeignKey(m => m.RestaurantId);

        builder.HasMany(r => r.Reservations)
            .WithOne(reserv => reserv.Restaurant)
            .HasForeignKey(reserv => reserv.RestaurantId);

        builder.HasMany(r => r.Tables)
           .WithOne(t => t.Restaurant)
           .HasForeignKey(t => t.RestaurantId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(r => r.Address, a =>
        {
            a.Property(ad => ad.Street).IsRequired().HasMaxLength(200);
            a.Property(ad => ad.City).IsRequired().HasMaxLength(100);
            a.Property(ad => ad.Country).IsRequired().HasMaxLength(100);
            a.Property(ad => ad.State).HasMaxLength(100);
            a.Property(ad => ad.Latitude).IsRequired();
            a.Property(ad => ad.Longitude).IsRequired();
        });
    }
}
