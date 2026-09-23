using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace OnlineStore.DataAccess
{
    public class ProductCategoryDataAccess
    {
        private readonly string _connectionString =
            ConfigurationManager
                .ConnectionStrings["OnlineStoreConnection"]
                .ConnectionString;

        public ProductCategory GetById(int categoryId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT CategoryID, CategoryName
                    FROM ProductCategory
                    WHERE CategoryID = @CategoryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int)
                                      .Value = categoryId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ProductCategory
                            {
                                CategoryID = (int)reader["CategoryID"],
                                CategoryName = reader["CategoryName"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }
        public List<ProductCategory> GetAll()
        {
            List<ProductCategory> categories = new List<ProductCategory>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT CategoryID, CategoryName
            FROM ProductCategory";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new ProductCategory
                            {
                                CategoryID = (int)reader["CategoryID"],
                                CategoryName = reader["CategoryName"].ToString()
                            });
                        }
                    }
                }
            }

            return categories;
        }
        public int Add(ProductCategory category)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO ProductCategory (CategoryName)
            VALUES (@CategoryName);

            SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CategoryName", System.Data.SqlDbType.NVarChar, 100)
                                          .Value = category.CategoryName;

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }

        }
        public bool Update(ProductCategory category)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE ProductCategory
            SET CategoryName = @CategoryName
            WHERE CategoryID = @CategoryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int)
                                      .Value = category.CategoryID;

                    command.Parameters.Add("@CategoryName", System.Data.SqlDbType.NVarChar, 100)
                                      .Value = category.CategoryName;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Delete(int categoryId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            DELETE FROM ProductCategory
            WHERE CategoryID = @CategoryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int)
                        .Value = categoryId;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Exists(int categoryId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(1)
            FROM ProductCategory
            WHERE CategoryID = @CategoryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int)
                        .Value = categoryId;

                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
        public bool ExistsByName(string categoryName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(1)
            FROM ProductCategory
            WHERE CategoryName = @CategoryName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CategoryName", System.Data.SqlDbType.NVarChar, 100)
                        .Value = categoryName;

                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
        public bool NameExistsForAnotherCategory(string categoryName, int categoryId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(1)
            FROM ProductCategory
            WHERE CategoryName = @CategoryName
              AND CategoryID <> @CategoryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CategoryName", System.Data.SqlDbType.NVarChar, 100)
                        .Value = categoryName;

                    command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int)
                        .Value = categoryId;

                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
        public bool HasProducts(int categoryId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(1)
            FROM ProductCatalog
            WHERE CategoryID = @CategoryID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CategoryID", System.Data.SqlDbType.Int)
                        .Value = categoryId;

                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
    }
}
