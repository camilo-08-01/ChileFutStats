using ChileFutStats.DTOs;
using ChileFutStats.Repositories;
using ChileFutStats.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChileFutStats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidoController : ControllerBase
    {
        private readonly PartidoRepository _partidoRepository;
        private readonly TemporadaRepository _temporadaRepository;
        private readonly SofascoreService _sofascoreService;

        public PartidoController(PartidoRepository partidoRepository, TemporadaRepository temporadaRepository, SofascoreService sofascoreService)
        {
            _partidoRepository = partidoRepository;
            _temporadaRepository = temporadaRepository;
            _sofascoreService = sofascoreService;
        }

        [HttpPost("sincronizar")]
        public async Task<IActionResult> Sincronizar(int tournamentId = 11653, int seasonId = 88493)
        {
            var lastMatches = await _sofascoreService.GetLastMatchesAsync(tournamentId, seasonId);
            var currentMatches = await _sofascoreService.GetMatchesAsync(tournamentId, seasonId);
            var nextMatches = await _sofascoreService.GetNextMatchesAsync(tournamentId, seasonId);

            await SincronizarTemporada(lastMatches ?? currentMatches ?? nextMatches, seasonId, tournamentId);

            var totalSincronizados = 0;
            totalSincronizados += await SincronizarPagina(lastMatches, seasonId);
            totalSincronizados += await SincronizarPagina(currentMatches, seasonId);
            totalSincronizados += await SincronizarPagina(nextMatches, seasonId);

            return Ok(new { mensaje = $"{totalSincronizados} partidos sincronizados correctamente." });
        }

        private async Task SincronizarTemporada(SofascoreMatchesResponse? response, int seasonId, int tournamentId)
        {
            var primerEvento = response?.Events.FirstOrDefault();

            var temporada = new Models.Temporada
            {
                TemporadaId = seasonId,
                UniqueTournamentId = tournamentId,
                NombreTorneo = primerEvento?.Tournament?.Name,
                Anio = int.TryParse(primerEvento?.Season?.Year, out var anio) ? anio : null
            };

            await _temporadaRepository.UpsertAsync(temporada);
        }

        private async Task<int> SincronizarPagina(SofascoreMatchesResponse? response, int temporadaId)
        {
            if (response == null || response.Events.Count == 0)
            {
                return 0;
            }

            foreach (var evento in response.Events)
            {
                var partido = new Models.Partido
                {
                    PartidoId = evento.Id,
                    TemporadaId = temporadaId,
                    EquipoLocalId = evento.HomeTeam.Id,
                    EquipoVisitaId = evento.AwayTeam.Id,
                    Jornada = evento.RoundInfo?.Round,
                    FechaHora = DateTimeOffset.FromUnixTimeSeconds(evento.StartTimestamp).UtcDateTime,
                    GolesLocal = evento.HomeScore?.Current,
                    GolesVisita = evento.AwayScore?.Current,
                    Estado = evento.Status.Type
                };

                await _partidoRepository.UpsertAsync(partido);
            }

            return response.Events.Count;
        }
    }
}