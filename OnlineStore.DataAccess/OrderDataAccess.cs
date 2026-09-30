using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using static OnlineStore.Models.Order;

namespace OnlineStore.DataAccess
{
    public class OrderDataAccess
    {
        private readonly string _connectionString =
            ConfigurationManager
                .ConnectionStrings["OnlineStoreConnection"]
                .ConnectionString;

        public Order GetById(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT OrderID, CustomerID, OrderDate,
                           TotalAmount, Status
                    FROM Orders
                    WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                                      .Value = orderId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Order
                            {
                                OrderID = (int)reader["OrderID"],
                                CustomerID = (int)reader["CustomerID"],
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalAmount = (decimal)reader["TotalAmount"],
                                Status = (OrderStatus)Enum.Parse(typeof(OrderStatus),reader["Status"].ToString())
                            };
                        }
                    }
                }
            }

            return null;
        }
    

        public List<Order> GetAll()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT OrderID, CustomerID, OrderDate,
                   TotalAmount, Status
            FROM Orders";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderID = (int)reader["OrderID"],
                                CustomerID = (int)reader["CustomerID"],
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalAmount = (decimal)reader["TotalAmount"],
                                Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), reader["Status"].ToString())
                            });
                        }
                    }
                }
            }

            return orders;
        }
        public int Add(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO Orders
            (
                CustomerID,
                OrderDate,
                TotalAmount,
                Status
            )
            VALUES
            (
                @CustomerID,
                @OrderDate,
                @TotalAmount,
                @Status
            );

            SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CustomerID", System.Data.SqlDbType.Int)
                                      .Value = order.CustomerID;

                    command.Parameters.Add("@OrderDate", System.Data.SqlDbType.DateTime)
                                      .Value = order.OrderDate;

                    command.Parameters.Add("@TotalAmount", System.Data.SqlDbType.Decimal)
                                      .Value = order.TotalAmount;

                    command.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 50)
                                      .Value = order.Status.ToString();

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public bool Update(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE Orders
            SET OrderDate = @OrderDate,
                TotalAmount = @TotalAmount,
                Status = @Status
            WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                                      .Value = order.OrderID;

                    command.Parameters.Add("@OrderDate", System.Data.SqlDbType.DateTime)
                                      .Value = order.OrderDate;

                    command.Parameters.Add("@TotalAmount", System.Data.SqlDbType.Decimal)
                                      .Value = order.TotalAmount;

                    command.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 50)
                                      .Value = order.Status.ToString();

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Cancel(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE Orders
            SET Status = @Status
            WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                        .Value = orderId;

                    command.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 50)
                        .Value = nameof(OrderStatus.Cancelled);

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool HasOrdersByCustomerId(int customerId)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"
            SELECT COUNT(*)
            FROM Orders
            WHERE CustomerID = @CustomerID";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.Add("@CustomerID",System.Data.SqlDbType.Int)
                            .Value = customerId;

            connection.Open();

            return (int)command.ExecuteScalar() > 0;
        }
    }
}
        public bool Exists(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(1)
            FROM Orders
            WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                        .Value = orderId;

                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
        public bool Confirm(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE Orders
            SET Status = 'Confirmed'
            WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                        .Value = orderId;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool IsConfirmed(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(*)
            FROM Orders
            WHERE OrderID = @OrderID
              AND Status = 'Confirmed'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                        .Value = orderId;

                    connection.Open();

                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
        
    }
}