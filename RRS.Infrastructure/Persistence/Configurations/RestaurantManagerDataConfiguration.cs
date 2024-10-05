using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRS.Core.Models;

public class RestaurantManagerDataConfiguration : IEntityTypeConfiguration<RestaurantManagerData>
{
    public void Configure(EntityTypeBuilder<RestaurantManagerData> builder)
    {
        builder.HasKey(rm => rm.Id);

        builder.HasOne(rm => rm.AppUser)
            .WithOne(u => u.RestaurantManagerData)
            .HasForeignKey<RestaurantManagerData>(rm => rm.AppUserId);

        builder.HasOne(rm => rm.Restaurant)
            .WithMany(r => r.Manageres)
            .HasForeignKey(rm => rm.RestaurantId); 
    }
}
