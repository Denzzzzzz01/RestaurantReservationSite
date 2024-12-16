using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RRS.Core.Models;

namespace RRS.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.HasIndex(n => n.UserId);
        builder.HasIndex(n => n.RestaurantId);

        builder.HasOne(n => n.User)
               .WithMany(u => u.Notifications)
               .HasForeignKey(n => n.UserId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(n => n.Restaurant)
               .WithMany(r => r.Notifications)
               .HasForeignKey(n => n.RestaurantId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.Property(n => n.Message)
           .IsRequired()
           .HasMaxLength(500);
    }
}
