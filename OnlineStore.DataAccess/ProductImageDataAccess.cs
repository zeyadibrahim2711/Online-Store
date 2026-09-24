using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace OnlineStore.DataAccess
{
    public class ProductImageDataAccess
    {
        private readonly string _connectionString =
            ConfigurationManager
                .ConnectionStrings["OnlineStoreConnection"]
                .ConnectionString;

        public List<ProductImage> GetByProductId(int productId)
        {
            List<ProductImage> images = new List<ProductImage>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT *
                    FROM ProductImages
                    WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int)
                                      .Value = productId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            images.Add(new ProductImage
                            {
                                ImageID = (int)reader["ImageID"],
                                ProductID = (int)reader["ProductID"],
                                ImageURL = reader["ImageURL"].ToString(),
                                Order =(int)reader["Order"]
                            });
                        }
                    }
                }
            }

            return images;
        }
        public ProductImage GetByImageId(int imageId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT *
            FROM ProductImages
            WHERE ImageID = @ImageID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ImageID", System.Data.SqlDbType.Int)
                        .Value = imageId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ProductImage
                            {
                                ImageID = (int)reader["ImageID"],
                                ProductID = (int)reader["ProductID"],
                                ImageURL = reader["ImageURL"].ToString(),
                                Order =(int)reader["Order"]
                            };
                        }
                    }
                }
            }

            return null;
        }
        public int Add(ProductImage image)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO ProductImages
            (
                ProductID,
                ImageURL,
                [Order]
            )
            VALUES
            (
                @ProductID,
                @ImageURL,
                @Order
            );

            SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ProductID", System.Data.SqlDbType.Int)
                        .Value = image.ProductID;

                    command.Parameters.Add("@ImageURL", System.Data.SqlDbType.NVarChar, 500)
                        .Value = image.ImageURL;
                    
                    
                    command.Parameters.Add("@Order", System.Data.SqlDbType.Int)
                        .Value = image.Order;
                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public bool Update(ProductImage image)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE ProductImages
            SET ImageURL = @ImageURL,
                [Order] = @Order
            WHERE ImageID = @ImageID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ImageID", System.Data.SqlDbType.Int)
                        .Value = image.ImageID;

                    command.Parameters.Add("@ImageURL", System.Data.SqlDbType.NVarChar, 500)
                        .Value = image.ImageURL;

                    command.Parameters.Add("@Order", System.Data.SqlDbType.Int)
                        .Value = image.Order;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Delete(int imageId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            DELETE FROM ProductImages
            WHERE ImageID = @ImageID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ImageID", System.Data.SqlDbType.Int)
                                      .Value = imageId;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Exists(int imageId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(1)
            FROM ProductImages
            WHERE ImageID = @ImageID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ImageID", System.Data.SqlDbType.Int)
                        .Value = imageId;

                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
    }
}