using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RRS.Core.Models;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.SeatingCapacity)
            .IsRequired();

        builder.HasMany(r => r.Manageres)
            .WithOne(m => m.Restaurant)
            .HasForeignKey(m => m.RestaurantId);


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
