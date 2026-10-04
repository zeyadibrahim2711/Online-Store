using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OnlineStore.DataAccess
{
    public class CartDataAccess
    {
        private readonly string _connectionString =
            ConfigurationManager
                .ConnectionStrings["OnlineStoreConnection"]
                .ConnectionString;

        public int Add(Cart cart)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Carts
                    (
                        CustomerID
                    )
                    VALUES
                    (
                        @CustomerID
                    );

                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CustomerID", SqlDbType.Int)
                        .Value = cart.CustomerID;

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public Cart GetById(int cartId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT CartID,
                   CustomerID,
                   CreatedDate
            FROM Carts
            WHERE CartID = @CartID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CartID", SqlDbType.Int)
                        .Value = cartId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cart
                            {
                                CartID = (int)reader["CartID"],
                                CustomerID = (int)reader["CustomerID"],
                                CreatedDate = (DateTime)reader["CreatedDate"]
                            };
                        }
                    }
                }
            }

            return null;
        }
        public bool IsCartExists(int cartId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(*)
            FROM Carts
            WHERE CartID = @CartID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CartID", SqlDbType.Int)
                        .Value = cartId;
                    
                    connection.Open();

                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
        public Cart GetByCustomerId(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT CartID,
                   CustomerID,
                   CreatedDate
            FROM Carts
            WHERE CustomerID = @CustomerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CustomerID", SqlDbType.Int)
                        .Value = customerId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cart
                            {
                                CartID = (int)reader["CartID"],
                                CustomerID = (int)reader["CustomerID"],
                                CreatedDate = (DateTime)reader["CreatedDate"]
                            };
                        }
                    }
                }
            }
            return null;
        }
        public List<Cart> GetAll()
        {
            List<Cart> carts = new List<Cart>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT CartID,
                   CustomerID,
                   CreatedDate
            FROM Carts";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            carts.Add(new Cart
                            {
                                CartID = (int)reader["CartID"],
                                CustomerID = (int)reader["CustomerID"],
                                CreatedDate = (DateTime)reader["CreatedDate"]
                            });
                        }
                    }
                }
            }

            return carts;
        }
        
    }
}