namespace Zad3;

public interface IWarehouseRepository
{
    bool CheckIfProductExists(int productId);
    bool CheckIfWarehouseExists(int warehouseId);
    bool CheckIfOrderExists(int productId, int amount, DateTime createdAt);
    bool CheckIfOrderFulfilled(int orderId);
    void UpdateOrderFulfilledAt(int orderId);
    int InsertProductWarehouse(int warehouseId, int productId, int orderId, int amount);
}