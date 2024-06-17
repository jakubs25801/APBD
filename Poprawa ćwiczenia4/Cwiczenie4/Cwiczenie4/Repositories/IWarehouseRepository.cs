using Cwiczenie4.Models;

namespace Cwiczenie4.Repositories;

public interface IWarehouseRepository
{
    Task<Warehouse> GetByIdAsync(int id);
}