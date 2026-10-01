using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using static OnlineStore.Models.Shipping;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OnlineStore.DataAccess
{
    public class ShippingDataAccess
    {
        private readonly string _connectionString =
        ConfigurationManager
            .ConnectionStrings["OnlineStoreConnection"]
            .ConnectionString;
        public Shipping GetById(int shippingId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT ShippingID,
                   OrderID,
                   CarrierName,
                   TrackingNumber,
                   Status,
                   EstimatedDeliveryDate,
                   ActualDeliveryDate
            FROM Shippings
            WHERE ShippingID = @ShippingID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ShippingID", SqlDbType.Int)
                                      .Value = shippingId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Shipping
                            {
                                ShippingID = (int)reader["ShippingID"],
                                OrderID = (int)reader["OrderID"],
                                CarrierName = reader["CarrierName"].ToString(),
                                TrackingNumber = reader["TrackingNumber"].ToString(),
                                Status = (ShippingStatus)Enum.Parse(typeof(Shipping.ShippingStatus)
                                ,reader["Status"].ToString()),
                                EstimatedDeliveryDate =
                                    (DateTime)reader["EstimatedDeliveryDate"],
                                ActualDeliveryDate =
                                    reader["ActualDeliveryDate"] == DBNull.Value
                                        ? null
                                        : (DateTime?)reader["ActualDeliveryDate"]
                            };
                        }
                    }
                }
            }
            return null;
        }
        public Shipping GetByOrderId(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT ShippingID,
                   OrderID,
                   CarrierName,
                   TrackingNumber,
                   Status,
                   EstimatedDeliveryDate,
                   ActualDeliveryDate
            FROM Shippings
            WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", SqlDbType.Int)
                                      .Value = orderId;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Shipping
                            {
                                ShippingID = (int)reader["ShippingID"],
                                OrderID = (int)reader["OrderID"],
                                CarrierName = reader["CarrierName"].ToString(),
                                TrackingNumber = reader["TrackingNumber"].ToString(),
                                Status = (ShippingStatus)Enum.Parse(typeof(Shipping.ShippingStatus)
                                , reader["Status"].ToString()),
                                EstimatedDeliveryDate =
                                    (DateTime)reader["EstimatedDeliveryDate"],
                                ActualDeliveryDate =
                                    reader["ActualDeliveryDate"] == DBNull.Value
                                        ? null
                                        : (DateTime?)reader["ActualDeliveryDate"]
                            };
                        }
                    }
                }
            }

            return null;
        }
        public List<Shipping> GetAll()
        {
            List<Shipping> shippings = new List<Shipping>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT ShippingID,
                   OrderID,
                   CarrierName,
                   TrackingNumber,
                   Status,
                   EstimatedDeliveryDate,
                   ActualDeliveryDate
            FROM Shippings";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shippings.Add(new Shipping
                            {
                                ShippingID = (int)reader["ShippingID"],
                                OrderID = (int)reader["OrderID"],
                                CarrierName = reader["CarrierName"].ToString(),
                                TrackingNumber = reader["TrackingNumber"].ToString(),
                                Status = (Shipping.ShippingStatus)Enum.Parse(
                                    typeof(Shipping.ShippingStatus),
                                    reader["Status"].ToString()),
                                EstimatedDeliveryDate =
                                    (DateTime)reader["EstimatedDeliveryDate"],
                                ActualDeliveryDate =
                                    reader["ActualDeliveryDate"] == DBNull.Value
                                        ? null
                                        : (DateTime?)reader["ActualDeliveryDate"]
                            });
                        }
                    }
                }
            }

            return shippings;
        }
        public int Add(Shipping shipping)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO Shippings
            (
                OrderID,
                CarrierName,
                TrackingNumber,
                Status,
                EstimatedDeliveryDate,
                ActualDeliveryDate
            )
            VALUES
            (
                @OrderID,
                @CarrierName,
                @TrackingNumber,
                @Status,
                @EstimatedDeliveryDate,
                @ActualDeliveryDate
            );

            SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", SqlDbType.Int)
                                      .Value = shipping.OrderID;

                    command.Parameters.Add("@CarrierName", SqlDbType.NVarChar, 100)
                                      .Value = shipping.CarrierName;

                    command.Parameters.Add("@TrackingNumber", SqlDbType.NVarChar, 100)
                                      .Value = shipping.TrackingNumber;

                    command.Parameters.Add("@Status", SqlDbType.NVarChar, 50)
                                      .Value = shipping.Status.ToString();

                    command.Parameters.Add("@EstimatedDeliveryDate", SqlDbType.DateTime)
                                      .Value = shipping.EstimatedDeliveryDate;

                    command.Parameters.Add("@ActualDeliveryDate", SqlDbType.DateTime);

                    if (shipping.ActualDeliveryDate.HasValue)
                        command.Parameters["@ActualDeliveryDate"].Value =
                            shipping.ActualDeliveryDate.Value;
                    else
                        command.Parameters["@ActualDeliveryDate"].Value =
                            DBNull.Value;

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public bool Update(Shipping shipping)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE Shippings
            SET CarrierName = @CarrierName,
                TrackingNumber = @TrackingNumber,
                Status = @Status,
                EstimatedDeliveryDate = @EstimatedDeliveryDate,
                ActualDeliveryDate = @ActualDeliveryDate
            WHERE ShippingID = @ShippingID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ShippingID", SqlDbType.Int)
                                      .Value = shipping.ShippingID;

                    command.Parameters.Add("@CarrierName", SqlDbType.NVarChar, 100)
                                      .Value = shipping.CarrierName;

                    command.Parameters.Add("@TrackingNumber", SqlDbType.NVarChar, 100)
                                      .Value = shipping.TrackingNumber;

                    command.Parameters.Add("@Status", SqlDbType.NVarChar, 50)
                                      .Value = shipping.Status.ToString();

                    command.Parameters.Add("@EstimatedDeliveryDate", SqlDbType.DateTime)
                                      .Value = shipping.EstimatedDeliveryDate;

                    command.Parameters.Add("@ActualDeliveryDate", SqlDbType.DateTime);

                    if (shipping.ActualDeliveryDate.HasValue)
                        command.Parameters["@ActualDeliveryDate"].Value =
                            shipping.ActualDeliveryDate.Value;
                    else
                        command.Parameters["@ActualDeliveryDate"].Value =
                            DBNull.Value;

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Cancel(int shippingId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            UPDATE Shippings
            SET Status = @Status
            WHERE ShippingID = @ShippingID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ShippingID", SqlDbType.Int)
                        .Value = shippingId;

                    command.Parameters.Add("@Status", SqlDbType.NVarChar, 50)
                        .Value = Shipping.ShippingStatus.Cancelled.ToString();

                    connection.Open();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Exists(int shippingId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(*)
            FROM Shippings
            WHERE ShippingID = @ShippingID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ShippingID", SqlDbType.Int)
                        .Value = shippingId;

                    connection.Open();

                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
        public bool ExistsActiveByOrderId(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT COUNT(*)
            FROM Shippings
            WHERE OrderID = @OrderID
              AND Status != @Status";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@OrderID", SqlDbType.Int)
                        .Value = orderId;

                    command.Parameters.Add("@Status", SqlDbType.NVarChar, 50)
                        .Value = Shipping.ShippingStatus.Cancelled.ToString();

                    connection.Open();

                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
    }
}
