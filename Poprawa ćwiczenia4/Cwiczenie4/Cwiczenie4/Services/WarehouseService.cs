using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cwiczenie4.Data;
using Cwiczenie4.Models;
using Cwiczenie4.Repositories;
using Microsoft.Data.SqlClient;

namespace Cwiczenie4.Services;

public class WarehouseService : IWarehouseService
    {
        private readonly IProductRepository _productRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly ApplicationDbContext _context;

        public WarehouseService(
            IProductRepository productRepository,
            IWarehouseRepository warehouseRepository,
            IOrderRepository orderRepository,
            IProductWarehouseRepository productWarehouseRepository,
            ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _orderRepository = orderRepository;
            _productWarehouseRepository = productWarehouseRepository;
            _context = context;
        }

        public async Task<(bool IsSuccess, string ErrorMessage, int NewProductWarehouseId)> AddProductToWarehouseAsync(AddProductRequest request)
        {
            // Sprawdzenie, czy produkt istnieje
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return (false, "Product not found", 0);
            }

            // Sprawdzenie, czy magazyn istnieje
            var warehouse = await _warehouseRepository.GetByIdAsync(request.WarehouseId);
            if (warehouse == null)
            {
                return (false, "Warehouse not found", 0);
            }

            // Sprawdzenie, czy ilość jest większa niż 0
            if (request.Amount <= 0)
            {
                return (false, "Amount must be greater than 0", 0);
            }

            // Sprawdzenie, czy istnieje odpowiednie zamówienie
            var order = await _orderRepository.GetOrderAsync(request.ProductId, request.Amount, request.CreatedAt);
            if (order == null)
            {
                return (false, "Order not found or invalid", 0);
            }

            // Sprawdzenie, czy zamówienie zostało już zrealizowane
            if (await _orderRepository.IsOrderFulfilledAsync(order.Id))
            {
                return (false, "Order already fulfilled", 0);
            }

            // Aktualizacja zamówienia
            order.CreatedAt = DateTime.UtcNow;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            // Wstawienie rekordu do Product_Warehouse
            var productWarehouse = new ProductWarehouse
            {
                OrderId = order.Id,
                WarehouseId = request.WarehouseId,
                Price = product.Price * request.Amount,
                CreatedAt = DateTime.UtcNow,
                FulfilledAt = DateTime.UtcNow
            };

            var newProductWarehouseId = await _productWarehouseRepository.AddAsync(productWarehouse);

            return (true, string.Empty, newProductWarehouseId);
        }

        public async Task<(bool IsSuccess, string ErrorMessage, int NewProductWarehouseId)> AddProductToWarehouseUsingProcAsync(AddProductRequest request)
        {
            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("AddProductToWarehouse", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductId", request.ProductId);
                    command.Parameters.AddWithValue("@WarehouseId", request.WarehouseId);
                    command.Parameters.AddWithValue("@Amount", request.Amount);
                    command.Parameters.AddWithValue("@CreatedAt", request.CreatedAt);

                    await connection.OpenAsync();

                    var result = await command.ExecuteScalarAsync();
                    if (result != null && int.TryParse(result.ToString(), out int newProductWarehouseId))
                    {
                        return (true, string.Empty, newProductWarehouseId);
                    }
                    return (false, "Error executing stored procedure", 0);
                }
            }
        }
    }