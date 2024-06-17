using Cwiczenie4.Data;
using Cwiczenie4.Models;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenie4.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Order> GetOrderAsync(int productId, int amount, DateTime createdAt)
    {
        return await _context.Orders.FirstOrDefaultAsync(o =>
            o.ProductId == productId &&
            o.Amount == amount &&
            o.CreatedAt < createdAt);
    }

    public async Task<bool> IsOrderFulfilledAsync(int orderId)
    {
        return await _context.ProductWarehouses.AnyAsync(pw => pw.OrderId == orderId);
    }
}