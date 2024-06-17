using System.Threading.Tasks;
using Cwiczenie4.Models;

namespace Cwiczenie4.Repositories;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(int d);
}