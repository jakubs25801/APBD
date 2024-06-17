using Cwiczenie4.Models;

namespace Cwiczenie4.Services;

public interface IWarehouseService
{
    Task<(bool IsSuccess, string ErrorMessage, int NewProductWarehouseId)> AddProductToWarehouseAsync(AddProductRequest request);
    Task<(bool IsSuccess, string ErrorMessage, int NewProductWarehouseId)> AddProductToWarehouseUsingProcAsync(AddProductRequest request);
}