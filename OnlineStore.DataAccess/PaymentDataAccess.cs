using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;



namespace OnlineStore.DataAccess
{
    public class PaymentDataAccess
    {
        private readonly string _connectionString =
         ConfigurationManager
             .ConnectionStrings["OnlineStoreConnection"]
             .ConnectionString;

        public Payment GetById(int paymentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT PaymentID, OrderID, Amount,
                   PaymentMethod, PaymentDate, Status
            FROM Payments
            WHERE PaymentID = @PaymentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PaymentID", System.Data.SqlDbType.Int)
                                      .Value = paymentId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Payment
                            {
                                PaymentID = (int)reader["PaymentID"],
                                OrderID = (int)reader["OrderID"],
                                Amount = (decimal)reader["Amount"],
                                PaymentMethod = reader["PaymentMethod"].ToString(),
                                PaymentDate = (DateTime)reader["PaymentDate"],
                                Status = (Payment.PaymentStatus)Enum.Parse(
                                    typeof(Payment.PaymentStatus),
                                    reader["Status"].ToString())
                            };
                        }
                    }
                }
            }

            return null;
        }
        public List<Payment> GetByOrderId(int orderId)
        {
            List<Payment> payments = new List<Payment>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT PaymentID, OrderID, Amount,
                   PaymentMethod, PaymentDate,Status
            FROM Payments
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
                            payments.Add(new Payment
                            {
                                PaymentID = (int)reader["PaymentID"],
                                OrderID = (int)reader["OrderID"],
                                Amount = (decimal)reader["Amount"],
                                PaymentMethod = reader["PaymentMethod"].ToString(),
                                PaymentDate = (DateTime)reader["PaymentDate"],
                                Status = (Payment.PaymentStatus)Enum.Parse(
                                    typeof(Payment.PaymentStatus),
                                    reader["Status"].ToString())
                            });
                        }
                    }
                }
            }

            return payments;
        }
        public List<Payment> GetAll()
        {
            List<Payment> payments = new List<Payment>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT PaymentID,
                   OrderID,
                   Amount,
                   PaymentMethod,
                   PaymentDate,
                   Status
            FROM Payments";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment
                            {
                                PaymentID = (int)reader["PaymentID"],
                                OrderID = (int)reader["OrderID"],
                                Amount = (decimal)reader["Amount"],
                                PaymentMethod = reader["PaymentMethod"].ToString(),
                                PaymentDate = (DateTime)reader["PaymentDate"],
                                Status = (Payment.PaymentStatus)Enum.Parse(
                                    typeof(Payment.PaymentStatus),
                                    reader["Status"].ToString())
                            });
                        }
                    }
                }
            }

            return payments;
        }
        public int Add(Payment payment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO Payments
            (
                OrderID,
                Amount,
                PaymentMethod,
                PaymentDate,
                Status
            )
            VALUES
            (
                @OrderID,
                @Amount,
                @PaymentMethod,
                @PaymentDate,
                @Status
            );

            SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                                      .Value = payment.OrderID;

                    command.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal)
                                      .Value = payment.Amount;

                    command.Parameters.Add("@PaymentMethod", System.Data.SqlDbType.NVarChar, 50)
                                      .Value = payment.PaymentMethod;

                    command.Parameters.Add("@PaymentDate", System.Data.SqlDbType.DateTime)
                                      .Value = payment.PaymentDate;
                    command.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 50)
                        .Value = payment.Status.ToString();

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public bool Update(Payment payment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE Payments
            SET PaymentMethod = @PaymentMethod,
                PaymentDate = @PaymentDate,
                Status = @Status
            WHERE PaymentID = @PaymentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PaymentID", System.Data.SqlDbType.Int)
                                      .Value = payment.PaymentID;

                    command.Parameters.Add("@PaymentMethod", System.Data.SqlDbType.NVarChar, 50)
                                      .Value = payment.PaymentMethod;

                    command.Parameters.Add("@PaymentDate", System.Data.SqlDbType.DateTime)
                                      .Value = payment.PaymentDate;
                    
                    command.Parameters.Add("@Status", System.Data.SqlDbType.NVarChar, 50)
                        .Value = payment.Status.ToString();

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool HasSuccessfulPayment(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(1)
            FROM Payments
            WHERE OrderID = @OrderID
              AND Status = 'Successful'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", System.Data.SqlDbType.Int)
                        .Value = orderId;

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

    }
}

