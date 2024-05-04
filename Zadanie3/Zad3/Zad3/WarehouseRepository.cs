namespace Zad3;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly string _connectionString;

    public WarehouseRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public bool CheckIfProductExists(int productId)
    {
        throw new NotImplementedException();
    }

    public bool CheckIfWarehouseExists(int warehouseId)
    {
        throw new NotImplementedException();
    }

    public bool CheckIfOrderExists(int productId, int amount, DateTime createdAt)
    {
        throw new NotImplementedException();
    }

    public bool CheckIfOrderFulfilled(int orderId)
    {
        throw new NotImplementedException();
    }

    public void UpdateOrderFulfilledAt(int orderId)
    {
        throw new NotImplementedException();
    }

    public int InsertProductWarehouse(int warehouseId, int productId, int orderId, int amount)
    {
        throw new NotImplementedException();
    }
}