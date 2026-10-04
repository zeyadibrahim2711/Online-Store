using System.Collections.Generic;
using OnlineStore.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OnlineStore.DataAccess
{
    public class CartItemDataAccess
    {
        private readonly string _connectionString =
            ConfigurationManager
                .ConnectionStrings["OnlineStoreConnection"]
                .ConnectionString;

        public bool Add(CartItem cartItem)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO CartItems
                    (
                        CartID,
                        ProductID,
                        Quantity
                    )
                    VALUES
                    (
                        @CartID,
                        @ProductID,
                        @Quantity
                    )";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CartID", SqlDbType.Int)
                        .Value = cartItem.CartID;

                    command.Parameters.Add("@ProductID", SqlDbType.Int)
                        .Value = cartItem.ProductID;

                    command.Parameters.Add("@Quantity", SqlDbType.Int)
                        .Value = cartItem.Quantity;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Update(CartItem cartItem)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE CartItems
            SET Quantity = @Quantity
            WHERE CartID = @CartID
              AND ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CartID", SqlDbType.Int)
                        .Value = cartItem.CartID;

                    command.Parameters.Add("@ProductID", SqlDbType.Int)
                        .Value = cartItem.ProductID;

                    command.Parameters.Add("@Quantity", SqlDbType.Int)
                        .Value = cartItem.Quantity;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Delete(int cartId, int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            DELETE FROM CartItems
            WHERE CartID = @CartID
              AND ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CartID", SqlDbType.Int)
                        .Value = cartId;

                    command.Parameters.Add("@ProductID", SqlDbType.Int)
                        .Value = productId;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public CartItem GetById(int cartId, int productId)
        {
            CartItem cartItem = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT CartID,
                   ProductID,
                   Quantity
            FROM CartItems
            WHERE CartID = @CartID
              AND ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CartID", SqlDbType.Int)
                        .Value = cartId;

                    command.Parameters.Add("@ProductID", SqlDbType.Int)
                        .Value = productId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cartItem = new CartItem
                            {
                                CartID = (int)reader["CartID"],
                                ProductID = (int)reader["ProductID"],
                                Quantity = (int)reader["Quantity"]
                            };
                        }
                    }
                }
            }

            return cartItem;
        }
        public List<CartItem> GetByCartId(int cartId)
        {
            List<CartItem> cartItems = new List<CartItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT CartID,
                   ProductID,
                   Quantity
            FROM CartItems
            WHERE CartID = @CartID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CartID", SqlDbType.Int)
                        .Value = cartId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cartItems.Add(new CartItem
                            {
                                CartID = (int)reader["CartID"],
                                ProductID = (int)reader["ProductID"],
                                Quantity = (int)reader["Quantity"]
                            });
                        }
                    }
                }
            }

            return cartItems;
        }
        public List<CartItem> GetByProductId(int productId)
        {
            List<CartItem> cartItems = new List<CartItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT CartID,
                   ProductID,
                   Quantity
            FROM CartItems
            WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ProductID", SqlDbType.Int)
                        .Value = productId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cartItems.Add(new CartItem
                            {
                                CartID = (int)reader["CartID"],
                                ProductID = (int)reader["ProductID"],
                                Quantity = (int)reader["Quantity"]
                            });
                        }
                    }
                }
            }

            return cartItems;
        }
       
        public bool IsProductExistsInCart(int cartId, int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(*)
            FROM CartItems
            WHERE CartID = @CartID
              AND ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CartID", SqlDbType.Int)
                        .Value = cartId;

                    command.Parameters.Add("@ProductID", SqlDbType.Int)
                        .Value = productId;

                    connection.Open();

                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
       
        
    }
}