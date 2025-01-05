namespace RRS.Application.Contracts.Table;

public class AddRestaurantTablesDto
{
    public int NumberOfTables { get; set; }  
    public int TableCapacity { get; set; }  
    public string? Description { get; set; } 
}
