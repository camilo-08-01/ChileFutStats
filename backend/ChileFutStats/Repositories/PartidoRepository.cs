using ChileFutStats.Data;
using ChileFutStats.Models;
using Oracle.ManagedDataAccess.Client;

namespace ChileFutStats.Repositories
{
    public class PartidoRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public PartidoRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task UpsertAsync(Partido partido)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                MERGE INTO Partido p
                USING (SELECT :PartidoId AS PartidoId FROM dual) src
                ON (p.PartidoId = src.PartidoId)
                WHEN MATCHED THEN
                    UPDATE SET
                        TemporadaId = :TemporadaId,
                        EquipoLocalId = :EquipoLocalId,
                        EquipoVisitaId = :EquipoVisitaId,
                        Jornada = :Jornada,
                        FechaHora = :FechaHora,
                        GolesLocal = :GolesLocal,
                        GolesVisita = :GolesVisita,
                        Estado = :Estado
                WHEN NOT MATCHED THEN
                    INSERT (PartidoId, TemporadaId, EquipoLocalId, EquipoVisitaId, Jornada, FechaHora, GolesLocal, GolesVisita, Estado)
                    VALUES (:PartidoId, :TemporadaId, :EquipoLocalId, :EquipoVisitaId, :Jornada, :FechaHora, :GolesLocal, :GolesVisita, :Estado)";

            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("PartidoId", partido.PartidoId));
            command.Parameters.Add(new OracleParameter("TemporadaId", partido.TemporadaId));
            command.Parameters.Add(new OracleParameter("EquipoLocalId", partido.EquipoLocalId));
            command.Parameters.Add(new OracleParameter("EquipoVisitaId", partido.EquipoVisitaId));
            command.Parameters.Add(new OracleParameter("Jornada", (object?)partido.Jornada ?? DBNull.Value));
            command.Parameters.Add(new OracleParameter("FechaHora", partido.FechaHora));
            command.Parameters.Add(new OracleParameter("GolesLocal", (object?)partido.GolesLocal ?? DBNull.Value));
            command.Parameters.Add(new OracleParameter("GolesVisita", (object?)partido.GolesVisita ?? DBNull.Value));
            command.Parameters.Add(new OracleParameter("Estado", partido.Estado));

            await command.ExecuteNonQueryAsync();
        }
    }
}