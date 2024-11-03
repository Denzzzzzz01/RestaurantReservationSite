using System.ComponentModel.DataAnnotations;

namespace RRS.Application.Contracts.Restaurant;

public class SearchRestaurantsDto
{
    [MinLength(3, ErrorMessage = "Search query must be at least 3 characters long.")]
    [MaxLength(100, ErrorMessage = "Search query cannot exceed 100 characters.")]
    public string Query { get; set; }
}
