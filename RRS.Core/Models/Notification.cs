namespace RRS.Core.Models;

public class Notification
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; } 
    public AppUser User { get; set; }

    public Guid? RestaurantId { get; set; } 
    public Restaurant Restaurant { get; set; }

    public Guid? RelatedEntityId { get; set; } 
    public string RelatedEntity { get; set; }

    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

