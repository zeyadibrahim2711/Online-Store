using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace OnlineStore.DataAccess
{
    public class ProductCatalogDataAccess
    {
        private readonly string _connectionString =
            ConfigurationManager
                .ConnectionStrings["OnlineStoreConnection"]
                .ConnectionString;

        public Product GetById(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT ProductID, ProductName, Description,
                           Price, QuantityInStock, CategoryID
                    FROM ProductCatalog
                    WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int)
                                      .Value = productId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductID = (int)reader["ProductID"],
                                ProductName = reader["ProductName"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (decimal)reader["Price"],
                                QuantityInStock = (int)reader["QuantityInStock"],
                                CategoryID = (int)reader["CategoryID"]
                            };
                        }
                    }
                }
            }

            return null;
        }
        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT ProductID, ProductName, Description,
                   Price, QuantityInStock, CategoryID
            FROM ProductCatalog";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = (int)reader["ProductID"],
                                ProductName = reader["ProductName"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (decimal)reader["Price"],
                                QuantityInStock = (int)reader["QuantityInStock"],
                                CategoryID = (int)reader["CategoryID"]
                            });
                        }
                    }
                }
            }

            return products;
        }
        public int Add(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO ProductCatalog
            (
                ProductName,
                Description,
                Price,
                QuantityInStock,
                CategoryID
            )
            VALUES
            (
                @ProductName,
                @Description,
                @Price,
                @QuantityInStock,
                @CategoryID
            );

            SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ProductName", System.Data.SqlDbType.NVarChar, 100)
                                      .Value = product.ProductName;

                    command.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 500)
                                      .Value = product.Description;

                    command.Parameters.Add("@Price", System.Data.SqlDbType.SmallMoney)
                                      .Value = product.Price;

                    command.Parameters.Add("@QuantityInStock", System.Data.SqlDbType.Int)
                                      .Value = product.QuantityInStock;

                    command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int)
                                      .Value = product.CategoryID;

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }

        }
        public bool Update(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE ProductCatalog
            SET ProductName = @ProductName,
                Description = @Description,
                Price = @Price,
                QuantityInStock = @QuantityInStock,
                CategoryID = @CategoryID
            WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int)
                                      .Value = product.ProductID;

                    command.Parameters.Add("@ProductName", System.Data.SqlDbType.NVarChar, 100)
                                      .Value = product.ProductName;

                    command.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 500)
                                      .Value = product.Description;

                    command.Parameters.Add("@Price", System.Data.SqlDbType.SmallMoney)
                                      .Value = product.Price;

                    command.Parameters.Add("@QuantityInStock", System.Data.SqlDbType.Int)
                                      .Value = product.QuantityInStock;

                    command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int)
                                      .Value = product.CategoryID;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
            
        }
        public bool Delete(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            DELETE FROM ProductCatalog
            WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int)
                        .Value = productId;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Exists(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(1)
            FROM ProductCatalog
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