using Cwiczenie4.Data;
using Cwiczenie4.Models;

namespace Cwiczenie4.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly ApplicationDbContext _context;

    public WarehouseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Warehouse> GetByIdAsync(int id)
    {
        return await _context.Warehouses.FindAsync(id);
    }
}