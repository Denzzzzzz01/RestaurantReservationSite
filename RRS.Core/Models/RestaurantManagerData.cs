using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RRS.Core.Models;

public class RestaurantManagerData
{
    public Guid Id { get; set; }

    [Required]
    public Guid AppUserId { get; set; }
    [Required]
    [ForeignKey("AppUserId")]
    public AppUser AppUser { get; set; }

    [Required]
    public Guid RestaurantId { get; set; }
    [Required]
    [ForeignKey("RestaurantId")]
    public Restaurant Restaurant { get; set; }
}
