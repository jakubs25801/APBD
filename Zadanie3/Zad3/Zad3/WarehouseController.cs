using System;
using System.Data.SqlClient;

namespace Zad3
{
    public class WarehouseController
    {
        private readonly string _connectionString;

        public WarehouseController(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string AddProductToWarehouse(ProductWarehouseRequest request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var productId = CheckIfProductExists(connection, request.IdProduct, transaction);
                        if (productId == null)
                        {
                            return "product not found";
                        }
                        var warehouseId = CheckIfWarehouseExists(connection, request.IdWarehouse, transaction);
                        if (warehouseId == null)
                        {
                            return "warehouse not found";
                        }

                        var orderId = CheckIfOrderExists(connection, request.IdProduct, request.Amount,
                            request.CreateAt, transaction);
                        if (orderId == null)
                        {
                            return "No valid order found for the product";
                        }

                        var orderFulfilled = CheckIfOrderFulfilled(connection, orderId.Value, transaction);
                        if (orderFulfilled == null)
                        {
                            return "Order has alredy benn fulfilled";
                        }
                        
                        UpdateOrderFulfilledAt(connection, orderId.Value, transaction);
                        InsertProductWarehouse(connection, request.IdWarehouse, request.IdProduct, orderId.Value, request.Amount, transaction);
                        
                        transaction.Commit();
                        return "Product added to warehouse successfully";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return $"An error occured: {ex.Message}";
                    }
                }
                
            }
        }
                private int? CheckIfProductExists(SqlConnection connection, int productId, SqlTransaction transaction)
                {
                    using (var command = new SqlCommand("SELECT IdProduct FROM Product WHERE IdProduct = @ProductId",
                               connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Convert.ToInt32(reader["IdProduct"]);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }

                private int? CheckIfWarehouseExists(SqlConnection connection, int warehouseId, SqlTransaction transaction)
                {
                    using (var command =
                           new SqlCommand("SELECT IdWarehouse FROM Warehouse WHERE IdWarehouse = @WarehouseId",
                               connection, transaction))
                    {
                        command.Parameters.AddWithValue("@WarehouseId", warehouseId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Convert.ToInt32(reader["IdWarehouse"]);
                            }
                            else
                            {
                                return null;
                            }
                            
                        }
                    }
                }

                private int? CheckIfOrderExists(SqlConnection connection, int productId, int amount, DateTime createdAt,
                    SqlTransaction transaction)
                {
                    using (var command =
                           new SqlCommand(
                               "SELECT IdOrder FROM Order WHERE IdProduct = @ProductId AND Amount = @Amount AND CreatedAt < @CreatedAt",
                               connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@CreatedAt", createdAt);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Convert.ToInt32(reader["IdOrder"]);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }

                private bool CheckIfOrderFulfilled(SqlConnection connection, int orderId, SqlTransaction transaction)
                {
                    using (var command =
                           new SqlCommand("SELECT IdOrder FROM Product_Warehouse WHERE IdOrder = @OrderId", connection,
                               transaction))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        using (var reader = command.ExecuteReader())
                        {
                                return reader.Read();
                        }
                    }
                }

                private void UpdateOrderFulfilledAt(SqlConnection connection, int orderId, SqlTransaction transaction)
                {
                    using (var command =
                           new SqlCommand("UPDATE Order SET FulfilledAt = GETDATE() WHERE IdOrder = @OrderId",
                               connection, transaction))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.ExecuteNonQuery();
                    }
                }

                private void InsertProductWarehouse(SqlConnection connection, int warehouseId, int productId, int orderId,
                    int amount, SqlTransaction transaction)
                {
                    var price = GetProductPrice(connection, productId);
                    using (var command =
                           new SqlCommand(
                               "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (@WarehouseId, @ProductId, @OrderId, @Amount, @Price, GETDATE())",
                               connection, transaction))
                    {
                        command.Parameters.AddWithValue("@WarehouseId", warehouseId);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@Price", price * amount);
                        command.ExecuteNonQuery();
                    }
                }

                private decimal GetProductPrice(SqlConnection connection, int productId)
                {
                    using (var command = new SqlCommand("SELECT Price FROM Product WHERE IdProduct = @ProductId",
                               connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        return Convert.ToDecimal(command.ExecuteScalar());
                    }
                    
                }
       
    }
}