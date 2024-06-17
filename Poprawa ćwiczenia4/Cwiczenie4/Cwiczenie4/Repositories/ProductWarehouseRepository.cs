using Cwiczenie4.Data;
using Cwiczenie4.Models;

namespace Cwiczenie4.Repositories;

public class ProductWarehouseRepository : IProductWarehouseRepository
{
    private readonly ApplicationDbContext _context;

    public ProductWarehouseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(ProductWarehouse productWarehouse)
    {
        _context.ProductWarehouses.Add(productWarehouse);
        await _context.SaveChangesAsync();
        return productWarehouse.Id;
    }
}