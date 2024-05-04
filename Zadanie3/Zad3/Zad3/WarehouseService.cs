namespace Zad3;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _repository;

    public WarehouseService(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public string AddProductToWarehouse(ProductWarehouseRequest request)
    {
        throw new NotImplementedException();
    }
}