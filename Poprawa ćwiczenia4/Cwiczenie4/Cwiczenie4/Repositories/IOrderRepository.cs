using Cwiczenie4.Models;

namespace Cwiczenie4.Repositories;

public interface IOrderRepository
{
    Task<Order> GetOrderAsync(int productId, int amount, DateTime createdAt);
    Task<bool> IsOrderFulfilledAsync(int orderId);
}