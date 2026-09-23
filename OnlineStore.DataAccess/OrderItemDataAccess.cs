using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace OnlineStore.DataAccess
{
    public class OrderItemDataAccess
    {
        private readonly string _connectionString =
            ConfigurationManager
                .ConnectionStrings["OnlineStoreConnection"]
                .ConnectionString;

        public List<OrderItem> GetByOrderId(int orderId)
        {
            List<OrderItem> items = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT  OrderID, ProductID,
                           Quantity, Price
                    FROM OrderItems
                    WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                                      .Value = orderId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new OrderItem
                            {
                                OrderItemID = (int)reader["OrderItemID"],
                                OrderID = (int)reader["OrderID"],
                                ProductID = (int)reader["ProductID"],
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"]
                            });
                        }
                    }
                }
            }

            return items;
        }
        public OrderItem GetById(int orderItemId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT  OrderID, ProductID,
                   Quantity, Price
            FROM OrderItems
            WHERE OrderItemID = @OrderItemID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderItemID", System.Data.SqlDbType.Int)
                                      .Value = orderItemId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new OrderItem
                            {
                                OrderItemID = (int)reader["OrderItemID"],
                                OrderID = (int)reader["OrderID"],
                                ProductID = (int)reader["ProductID"],
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"]
                            };
                        }
                    }
                }
            }

            return null;
        }
        public int Add(OrderItem item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO OrderItems
            (
                OrderID,
                ProductID,
                Quantity,
                Price
            )
            VALUES
            (
                @OrderID,
                @ProductID,
                @Quantity,
                @Price
            );

            SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                                      .Value = item.OrderID;

                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int)
                                      .Value = item.ProductID;

                    command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int)
                                      .Value = item.Quantity;

                    command.Parameters.Add("@Price", System.Data.SqlDbType.Decimal)
                                      .Value = item.Price;

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public bool Update(OrderItem item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE OrderItems
            SET ProductID = @ProductID,
                Quantity = @Quantity,
                Price = @Price
            WHERE OrderItemID = @OrderItemID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderItemID", System.Data.SqlDbType.Int)
                                      .Value = item.OrderItemID;

                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int)
                                      .Value = item.ProductID;

                    command.Parameters.Add("@Quantity", System.Data.SqlDbType.Int)
                                      .Value = item.Quantity;

                    command.Parameters.Add("@Price", System.Data.SqlDbType.Decimal)
                                      .Value = item.Price;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Delete(int orderItemId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            DELETE FROM OrderItems
            WHERE OrderItemID = @OrderItemID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderItemID", System.Data.SqlDbType.Int)
                                      .Value = orderItemId;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool HasOrders(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(1)
            FROM OrderItems
            WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int)
                        .Value = productId;

                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
    }
}