using RRS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace RRS.Core.Models;

public class Notification
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; } 
    public AppUser? User { get; set; }

    public Guid? RestaurantId { get; set; } 
    public Restaurant? Restaurant { get; set; }

    public Guid? RelatedEntityId { get; set; } 
    public string? RelatedEntityName { get; set; }

    [MaxLength(500, ErrorMessage="Maximum message length is 500!")]
    public string Message { get; set; }
    public NotificationType Type { get; set; } = NotificationType.General; 
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

