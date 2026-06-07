using Microsoft.Data.SqlClient;
using System.Configuration;

namespace DoAn_banPRO.Helpers
{
    public static class DbConnectionFactory
    {
        public static SqlConnection GetConnection()
        {
            // Set the connection string here directly for now, ideally in config
            string connectionString = @"Server=localhost;Database=QL_KHODIENTU;Trusted_Connection=True;TrustServerCertificate=True;";
            return new SqlConnection(connectionString);
        }
    }
}
