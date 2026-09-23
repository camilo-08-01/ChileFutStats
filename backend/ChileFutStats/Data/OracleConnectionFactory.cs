using Oracle.ManagedDataAccess.Client;

namespace ChileFutStats.Data
{
    public class OracleConnectionFactory
    {
        private readonly string _connectionString;

        public OracleConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleDb")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'OracleDb'.");
        }

        public OracleConnection CreateConnection()
        {
            return new OracleConnection(_connectionString);
        }
    }
}