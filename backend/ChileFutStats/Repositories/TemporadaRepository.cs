using ChileFutStats.Data;
using ChileFutStats.Models;
using Oracle.ManagedDataAccess.Client;

namespace ChileFutStats.Repositories
{
    public class TemporadaRepository
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public TemporadaRepository(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task UpsertAsync(Temporada temporada)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                MERGE INTO Temporada t
                USING (SELECT :TemporadaId AS TemporadaId FROM dual) src
                ON (t.TemporadaId = src.TemporadaId)
                WHEN MATCHED THEN
                    UPDATE SET
                        UniqueTournamentId = :UniqueTournamentId,
                        NombreTorneo = :NombreTorneo,
                        Anio = :Anio
                WHEN NOT MATCHED THEN
                    INSERT (TemporadaId, UniqueTournamentId, NombreTorneo, Anio)
                    VALUES (:TemporadaId, :UniqueTournamentId, :NombreTorneo, :Anio)";

            using var command = new OracleCommand(sql, connection);
            command.Parameters.Add(new OracleParameter("TemporadaId", temporada.TemporadaId));
            command.Parameters.Add(new OracleParameter("UniqueTournamentId", temporada.UniqueTournamentId));
            command.Parameters.Add(new OracleParameter("NombreTorneo", (object?)temporada.NombreTorneo ?? DBNull.Value));
            command.Parameters.Add(new OracleParameter("Anio", (object?)temporada.Anio ?? DBNull.Value));

            await command.ExecuteNonQueryAsync();
        }
    }
}