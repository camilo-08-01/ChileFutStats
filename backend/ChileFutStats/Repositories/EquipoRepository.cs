using ChileFutStats.Data;
using ChileFutStats.Models;
using Oracle.ManagedDataAccess.Client;

namespace ChileFutStats.Repositories
{
    public class EquipoRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public EquipoRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Equipo>> GetAllAsync()
        {
            var equipos = new List<Equipo>();

            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new OracleCommand("SELECT EquipoId, Nombre, Slug, NombreCorto, CodigoCorto, ColorPrimario, ColorSecundario FROM Equipo", connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                equipos.Add(new Equipo
                {
                    EquipoId = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Slug = reader.IsDBNull(2) ? null : reader.GetString(2),
                    NombreCorto = reader.IsDBNull(3) ? null : reader.GetString(3),
                    CodigoCorto = reader.IsDBNull(4) ? null : reader.GetString(4),
                    ColorPrimario = reader.IsDBNull(5) ? null : reader.GetString(5),
                    ColorSecundario = reader.IsDBNull(6) ? null : reader.GetString(6)
                });
            }

            return equipos;
        }

        public async Task UpsertAsync(Equipo equipo)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
        MERGE INTO Equipo e
        USING (SELECT :EquipoId AS EquipoId FROM dual) src
        ON (e.EquipoId = src.EquipoId)
        WHEN MATCHED THEN
            UPDATE SET
                Nombre = :Nombre,
                Slug = :Slug,
                NombreCorto = :NombreCorto,
                CodigoCorto = :CodigoCorto,
                ColorPrimario = :ColorPrimario,
                ColorSecundario = :ColorSecundario
        WHEN NOT MATCHED THEN
            INSERT (EquipoId, Nombre, Slug, NombreCorto, CodigoCorto, ColorPrimario, ColorSecundario)
            VALUES (:EquipoId, :Nombre, :Slug, :NombreCorto, :CodigoCorto, :ColorPrimario, :ColorSecundario)";

            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("EquipoId", equipo.EquipoId));
            command.Parameters.Add(new OracleParameter("Nombre", equipo.Nombre));
            command.Parameters.Add(new OracleParameter("Slug", (object?)equipo.Slug ?? DBNull.Value));
            command.Parameters.Add(new OracleParameter("NombreCorto", (object?)equipo.NombreCorto ?? DBNull.Value));
            command.Parameters.Add(new OracleParameter("CodigoCorto", (object?)equipo.CodigoCorto ?? DBNull.Value));
            command.Parameters.Add(new OracleParameter("ColorPrimario", (object?)equipo.ColorPrimario ?? DBNull.Value));
            command.Parameters.Add(new OracleParameter("ColorSecundario", (object?)equipo.ColorSecundario ?? DBNull.Value));

            await command.ExecuteNonQueryAsync();
        }
    }
}