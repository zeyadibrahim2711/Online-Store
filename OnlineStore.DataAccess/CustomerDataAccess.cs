using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OnlineStore.DataAccess
{
    public class CustomerDataAccess
    {
        private readonly string _connectionString =
            ConfigurationManager
                .ConnectionStrings["OnlineStoreConnection"]
                .ConnectionString;

        public Customer GetById(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT CustomerID, Name, Email, Phone, Address,
                           Username, Password
                    FROM Customers
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
                            return new Customer
                            {
                                CustomerID = (int)reader["CustomerID"],
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Address = reader["Address"].ToString(),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }
            public Customer GetByEmail(string email)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"
            SELECT CustomerID,
                   Name,
                   Email,
                   Phone,
                   Address
            FROM Customers
            WHERE Email = @Email";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100)
                              .Value = email;

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Customer
                    {
                        CustomerID = (int)reader["CustomerID"],
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString()
                    };
                }
            }
        }
    }

    return null;
}
    

        public List<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT CustomerID, Name, Email, Phone, Address,
                   Username, Password
            FROM Customers";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                CustomerID = (int)reader["CustomerID"],
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Address = reader["Address"].ToString(),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString()
                            });
                        }
                    }
                }
            }

            return customers;
        }
        public int Add(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO Customers
            (Name, Email, Phone, Address, Username, Password)
            VALUES
            (@Name, @Email, @Phone, @Address, @Username, @Password);

            SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 100)
                                          .Value = customer.Name;

                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 100)
                                          .Value = customer.Email;

                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20)
                                          .Value = customer.Phone;

                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 200)
                                          .Value = customer.Address;

                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 100)
                                          .Value = customer.Username;

                    command.Parameters.Add("@Password", SqlDbType.NVarChar, 100)
                                          .Value = customer.Password;

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public bool Update(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE Customers
            SET Name = @Name,
                Email = @Email,
                Phone = @Phone,
                Address = @Address,
                Username = @Username,
                Password = @Password
            WHERE CustomerID = @CustomerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CustomerID", SqlDbType.Int)
                                      .Value = customer.CustomerID;

                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 100)
                                      .Value = customer.Name;

                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 100)
                                      .Value = customer.Email;

                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20)
                                      .Value = customer.Phone;

                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 200)
                                      .Value = customer.Address;

                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 100)
                                      .Value = customer.Username;

                    command.Parameters.Add("@Password", SqlDbType.NVarChar, 100)
                                      .Value = customer.Password;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Delete(int customerId)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"
            DELETE FROM Customers
            WHERE CustomerID = @CustomerID";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;

            connection.Open();

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
public bool Exists(int customerId)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"
            SELECT COUNT(1)
            FROM Customers
            WHERE CustomerID = @CustomerID";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;

            connection.Open();

            int count = (int)command.ExecuteScalar();

            return count > 0;
        }
    }
}
public bool EmailExistsForAnotherCustomer(string email, int customerId)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = @"
            SELECT COUNT(1)
            FROM Customers
            WHERE Email = @Email
              AND CustomerID <> @CustomerID";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerId;

            connection.Open();

            int count = (int)command.ExecuteScalar();

            return count > 0;
        }
    }
}

    }
}