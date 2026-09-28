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
        public List<OrderItem> GetAll()
        {
            List<OrderItem> items = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT OrderID,
                   ProductID,
                   Quantity,
                   Price,
                   ReservationStatus,
                   ReservationExpiresAt
            FROM OrderItems";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new OrderItem
                            {
                                OrderID = (int)reader["OrderID"],
                                ProductID = (int)reader["ProductID"],
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"],
                                ReservationStatus = reader["ReservationStatus"].ToString(),
                                ReservationExpiresAt = reader["ReservationExpiresAt"] == DBNull.Value
                                    ? (DateTime?)null
                                    : (DateTime)reader["ReservationExpiresAt"]
                            });
                        }
                    }
                }
            }

            return items;
        }
        public List<OrderItem> GetByOrderId(int orderId)
        {
            List<OrderItem> items = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                 SELECT OrderID,
                   ProductID,
                   Quantity,
                   Price,
                   ReservationStatus,
                   ReservationExpiresAt
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
                                OrderID = (int)reader["OrderID"],
                                ProductID = (int)reader["ProductID"],
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"],
                                ReservationStatus = reader["ReservationStatus"].ToString(),
                                ReservationExpiresAt = reader["ReservationExpiresAt"] == DBNull.Value
                                    ? (DateTime?)null
                                    : (DateTime)reader["ReservationExpiresAt"]
                            });
                        }
                    }
                }
            }

            return items;
        }
        public OrderItem GetById(int orderId, int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
               SELECT OrderID,
                   ProductID,
                   Quantity,
                   Price,
                   ReservationStatus,
                   ReservationExpiresAt
            FROM OrderItems
            WHERE OrderID = @OrderID
              AND ProductID = @ProductID";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                        .Value = orderId;

                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int)
                        .Value = productId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new OrderItem
                            {
                                OrderID = (int)reader["OrderID"],
                                ProductID = (int)reader["ProductID"],
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"],
                                ReservationStatus = reader["ReservationStatus"].ToString(),
                                ReservationExpiresAt = reader["ReservationExpiresAt"] == DBNull.Value
                                    ? (DateTime?)null
                                    : (DateTime)reader["ReservationExpiresAt"]
                            };
                        }
                    }
                }
            }

            return null;
        }
        public bool Add(OrderItem item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO OrderItems
            (
                OrderID,
                ProductID,
                Quantity,
                Price,
                ReservationStatus,
                ReservationExpiresAt
            )
            VALUES
            (
                @OrderID,
                @ProductID,
                @Quantity,
                @Price,
                @ReservationStatus,
                @ReservationExpiresAt
            )";

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

                    command.Parameters.Add("@ReservationStatus", System.Data.SqlDbType.NVarChar, 50)
                        .Value = item.ReservationStatus;

                    command.Parameters.Add("@ReservationExpiresAt", System.Data.SqlDbType.DateTime)
                        .Value = item.ReservationExpiresAt.HasValue
                        ? (object)item.ReservationExpiresAt.Value
                        : DBNull.Value;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Update(OrderItem item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE OrderItems
            SET Quantity = @Quantity,
                Price = @Price,
                ReservationStatus = @ReservationStatus,
                ReservationExpiresAt = @ReservationExpiresAt
            WHERE OrderID = @OrderID
              AND ProductID = @ProductID";

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

                    command.Parameters.Add("@ReservationStatus", System.Data.SqlDbType.NVarChar, 50)
                        .Value = item.ReservationStatus;

                    command.Parameters.Add("@ReservationExpiresAt", System.Data.SqlDbType.DateTime)
                        .Value = item.ReservationExpiresAt.HasValue
                        ? (object)item.ReservationExpiresAt.Value
                        : DBNull.Value;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Delete(int orderId, int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            DELETE FROM OrderItems
            WHERE OrderID = @OrderID
              AND ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                        .Value = orderId;

                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int)
                        .Value = productId;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Exists(int orderId, int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(1)
            FROM OrderItems
            WHERE OrderID = @OrderID
              AND ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                        .Value = orderId;

                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int)
                        .Value = productId;

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
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