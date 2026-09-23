using ChileFutStats.Data;
using ChileFutStats.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace ChileFutStats.Repositories
{
    public class UsuarioRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public UsuarioRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT UsuarioId, NombreUsuario, Email, PasswordHash, FechaRegistro
                FROM Usuario
                WHERE NombreUsuario = :NombreUsuario";

            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("NombreUsuario", nombreUsuario));

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    UsuarioId = reader.GetInt32(0),
                    NombreUsuario = reader.GetString(1),
                    Email = reader.GetString(2),
                    PasswordHash = reader.GetString(3),
                    FechaRegistro = reader.GetDateTime(4)
                };
            }

            return null;
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = "SELECT COUNT(*) FROM Usuario WHERE Email = :Email";

            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("Email", email));

            var count = Convert.ToInt32(await command.ExecuteScalarAsync());
            return count > 0;
        }

        public async Task<int> CreateAsync(Usuario usuario)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                INSERT INTO Usuario (NombreUsuario, Email, PasswordHash, FechaRegistro)
                VALUES (:NombreUsuario, :Email, :PasswordHash, SYSDATE)
                RETURNING UsuarioId INTO :NuevoId";

            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("NombreUsuario", usuario.NombreUsuario));
            command.Parameters.Add(new OracleParameter("Email", usuario.Email));
            command.Parameters.Add(new OracleParameter("PasswordHash", usuario.PasswordHash));

            var idParam = new OracleParameter("NuevoId", OracleDbType.Int32)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            command.Parameters.Add(idParam);

            await command.ExecuteNonQueryAsync();

            return Convert.ToInt32(((OracleDecimal)idParam.Value).Value);
        }
    }
}