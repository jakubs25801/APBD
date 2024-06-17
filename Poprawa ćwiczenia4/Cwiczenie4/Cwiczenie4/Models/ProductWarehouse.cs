namespace Cwiczenie4.Models;

public class ProductWarehouse
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int WarehouseId { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime FulfilledAt { get; set; }

    public Order Order { get; set; }
    public Warehouse Warehouse { get; set; }
    
}