using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using OnlineStore.Models;

namespace OnlineStore.DataAccess
{
    public class ReviewDataAccess
    {
        private readonly string _connectionString =
            ConfigurationManager
                .ConnectionStrings["OnlineStoreConnection"]
                .ConnectionString;
        
        public Review GetById(int reviewId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT ReviewID,
                           ProductID,
                           CustomerID,
                           Rating,
                           ReviewText,
                           ReviewDate
                    FROM Reviews
                    WHERE ReviewID = @ReviewID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ReviewID", SqlDbType.Int)
                                      .Value = reviewId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Review
                            {
                                ReviewID = (int)reader["ReviewID"],
                                ProductID = (int)reader["ProductID"],
                                CustomerID = (int)reader["CustomerID"],
                                Rating = (int)reader["Rating"],
                                ReviewText = reader["ReviewText"].ToString(),
                                ReviewDate = (DateTime)reader["ReviewDate"]
                            };
                        }
                    }
                }
            }

            return null;
        }

        public List<Review> GetByProductId(int productId)
        {
            List<Review> reviews = new List<Review>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT ReviewID,
                           ProductID,
                           CustomerID,
                           Rating,
                           ReviewText,
                           ReviewDate
                    FROM Reviews
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
                            reviews.Add(new Review
                            {
                                ReviewID = (int)reader["ReviewID"],
                                ProductID = (int)reader["ProductID"],
                                CustomerID = (int)reader["CustomerID"],
                                Rating = (int)reader["Rating"],
                                ReviewText = reader["ReviewText"].ToString(),
                                ReviewDate = (DateTime)reader["ReviewDate"]
                            });
                        }
                    }
                }
            }

            return reviews;
        }

        public List<Review> GetByCustomerId(int customerId)
        {
            List<Review> reviews = new List<Review>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT ReviewID,
                           ProductID,
                           CustomerID,
                           Rating,
                           ReviewText,
                           ReviewDate
                    FROM Reviews
                    WHERE CustomerID = @CustomerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CustomerID", SqlDbType.Int)
                                      .Value = customerId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reviews.Add(new Review
                            {
                                ReviewID = (int)reader["ReviewID"],
                                ProductID = (int)reader["ProductID"],
                                CustomerID = (int)reader["CustomerID"],
                                Rating = (int)reader["Rating"],
                                ReviewText = reader["ReviewText"].ToString(),
                                ReviewDate = (DateTime)reader["ReviewDate"]
                            });
                        }
                    }
                }
            }

            return reviews;
        }

        public int Add(Review review)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Reviews
                    (
                        ProductID,
                        CustomerID,
                        Rating,
                        ReviewText
                    )
                    VALUES
                    (
                        @ProductID,
                        @CustomerID,
                        @Rating,
                        @ReviewText
                    );

                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ProductID", SqlDbType.Int)
                                      .Value = review.ProductID;

                    command.Parameters.Add("@CustomerID", SqlDbType.Int)
                                      .Value = review.CustomerID;

                    command.Parameters.Add("@Rating", SqlDbType.Int)
                                      .Value = review.Rating;

                    command.Parameters.Add("@ReviewText", SqlDbType.NVarChar, -1)
                                      .Value = review.ReviewText;

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public bool Update(Review review)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Reviews
                    SET Rating = @Rating,
                        ReviewText = @ReviewText
                    WHERE ReviewID = @ReviewID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ReviewID", SqlDbType.Int)
                                      .Value = review.ReviewID;

                    command.Parameters.Add("@Rating", SqlDbType.Int)
                                      .Value = review.Rating;

                    command.Parameters.Add("@ReviewText", SqlDbType.NVarChar, -1)
                                      .Value = review.ReviewText;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        
        public bool Exists(int reviewId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(*)
            FROM Reviews
            WHERE ReviewID = @ReviewID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ReviewID", SqlDbType.Int)
                        .Value = reviewId;

                    connection.Open();

                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
        public bool ExistsByCustomerAndProduct(int customerId, int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(*)
            FROM Reviews
            WHERE CustomerID = @CustomerID
              AND ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CustomerID", SqlDbType.Int)
                        .Value = customerId;

                    command.Parameters.Add("@ProductID", SqlDbType.Int)
                        .Value = productId;

                    connection.Open();

                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        public bool Delete(int reviewId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    DELETE FROM Reviews
                    WHERE ReviewID = @ReviewID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ReviewID", SqlDbType.Int)
                                      .Value = reviewId;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}