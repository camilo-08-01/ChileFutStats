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
        private readonly SofascoreService _sofascoreService;

        public PartidoController(PartidoRepository partidoRepository, SofascoreService sofascoreService)
        {
            _partidoRepository = partidoRepository;
            _sofascoreService = sofascoreService;
        }

        [HttpPost("sincronizar")]
        public async Task<IActionResult> Sincronizar(int tournamentId = 11653, int seasonId = 88493)
        {
            var totalSincronizados = 0;

            totalSincronizados += await SincronizarPagina(await _sofascoreService.GetLastMatchesAsync(tournamentId, seasonId), seasonId);
            totalSincronizados += await SincronizarPagina(await _sofascoreService.GetMatchesAsync(tournamentId, seasonId), seasonId);
            totalSincronizados += await SincronizarPagina(await _sofascoreService.GetNextMatchesAsync(tournamentId, seasonId), seasonId);

            return Ok(new { mensaje = $"{totalSincronizados} partidos sincronizados correctamente." });
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