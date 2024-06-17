using Cwiczenie4.Models;
using Cwiczenie4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenie4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToWarehouse([FromBody] AddProductRequest request)
    {
        var result = await _warehouseService.AddProductToWarehouseAsync(request);
        if (result.IsSuccess)
        {
            return Ok(result.NewProductWarehouseId);
        }
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("stored-procedure")]
    public async Task<IActionResult> AddProductToWarehouseUsingProc([FromBody] AddProductRequest request)
    {
        var result = await _warehouseService.AddProductToWarehouseUsingProcAsync(request);
        if (result.IsSuccess)
        {
            return Ok(result.NewProductWarehouseId);
        }
        return BadRequest(result.ErrorMessage);
    }
}