namespace Zad3;

public class ProductWarehouseRequest
{
    public int IdProduct { get; set; }
    public int IdWarehouse { get; set; }
    public int Amount { get; set; }
    public DateTime CreateAt { get; set; }
}