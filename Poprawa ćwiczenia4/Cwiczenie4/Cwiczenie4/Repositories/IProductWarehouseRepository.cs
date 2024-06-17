using Cwiczenie4.Models;

namespace Cwiczenie4.Repositories;

public interface IProductWarehouseRepository
{
    Task<int> AddAsync(ProductWarehouse productWarehouse);
}