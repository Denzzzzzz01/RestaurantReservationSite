namespace RRS.Application.Contracts.Table;

public class AddRestaurantTablesDto
{
    public Guid RestaurantId { get; set; }
    public int NumberOfTables { get; set; }  
    public int TableCapacity { get; set; }  
    public string? Description { get; set; } 
}
