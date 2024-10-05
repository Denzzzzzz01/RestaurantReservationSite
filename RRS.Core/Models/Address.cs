using System.ComponentModel.DataAnnotations;

namespace RRS.Core.Models;

public class Address
{

    [Required]
    [StringLength(200)]
    public string Street { get; set; }  

    [Required]
    [StringLength(100)]
    public string City { get; set; }

    [Required]
    [StringLength(100)]

    public string Country { get; set; }
    [StringLength(100)]
    public string? State { get; set; }


    [Required]
    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Required]
    [Range(-180, 180)]
    public double Longitude { get; set; }
}
