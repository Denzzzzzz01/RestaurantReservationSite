using Microsoft.EntityFrameworkCore;
using RRS.Application.Interfaces;
using RRS.Core.Models;

namespace RRS.Infrastructure.Repositories;


public class NotificationRepository : INotificationRepository
{
    private readonly IAppDbContext _dbContext;

    public NotificationRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddNotificationAsync(Notification notification, CancellationToken cancellationToken)
    {
        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Notification>> GetRetaurantNotificationsAsync(Guid restaurantId, CancellationToken cancellationToken)
    {
        return await _dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.RestaurantId == restaurantId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken)
    {
        var notification = await _dbContext.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteNotificationAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
    {
        var notification = await _dbContext.Notifications
            .FirstOrDefaultAsync(n => 
            n.Id == notificationId 
            && (n.UserId == userId || n.Restaurant.Manageres.Any(m => m.AppUserId == userId)), 
            cancellationToken)
            ?? throw new InvalidOperationException("Notification not found or you are not authorized to delete it.");

        _dbContext.Notifications.Remove(notification);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

}

