using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RRS.Core.Models;

namespace RRS.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.ReservationDate)
            .IsRequired();

        builder.Property(r => r.StartTime)
            .IsRequired();

        builder.Property(r => r.EndTime)
            .IsRequired();

        builder.Property(r => r.NumberOfSeats)
            .IsRequired();

        builder.HasOne(r => r.Table)
        .WithMany() 
        .HasForeignKey(r => r.TableId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
