using System.Configuration;
using System.Data.SqlClient;

namespace OnlineStore.DataAccess
{
    public static class DatabaseConnection
    {
        public static SqlConnection GetConnection()
        {
            string connectionString =
                ConfigurationManager
                .ConnectionStrings["OnlineStoreConnection"]
                .ConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}