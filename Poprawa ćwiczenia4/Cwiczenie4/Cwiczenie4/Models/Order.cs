namespace Cwiczenie4.Models;

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Amount { get; set; }
    public DateTime CreatedAt { get; set; }

    public Product Product { get; set; } 
}