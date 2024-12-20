namespace RRS.Application.Contracts.Notification;

public class RestaurantManagerInvitationDto
{
    public Guid RestaurantId { get; set; }
    public Guid RecipientId { get; set; }
}
