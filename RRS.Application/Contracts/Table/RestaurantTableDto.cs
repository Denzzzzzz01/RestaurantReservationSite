namespace RRS.Application.Contracts.Table;

public class RestaurantTableDto
{
    public Guid Id { get; set; }
    public int Capacity { get; set; }
    public int TableNumber { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
}
