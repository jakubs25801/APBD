namespace Cwiczenie4.Models;

public class AddProductRequest
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}