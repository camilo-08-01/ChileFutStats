using ChileFutStats.Repositories;
using ChileFutStats.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChileFutStats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipoController : ControllerBase
    {
        private readonly EquipoRepository _equipoRepository;
        private readonly SofascoreService _sofascoreService;

        public EquipoController(EquipoRepository equipoRepository, SofascoreService sofascoreService)
        {
            _equipoRepository = equipoRepository;
            _sofascoreService = sofascoreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var equipos = await _equipoRepository.GetAllAsync();
            return Ok(equipos);
        }

        [HttpPost("sincronizar")]
        public async Task<IActionResult> Sincronizar(int tournamentId = 11653, int seasonId = 88493)
        {
            var standings = await _sofascoreService.GetStandingsAsync(tournamentId, seasonId);

            if (standings == null || standings.Standings.Count == 0)
            {
                return StatusCode(502, new { error = "No se pudo obtener el standing desde Sofascore." });
            }

            var equiposSincronizados = 0;

            foreach (var row in standings.Standings[0].Rows)
            {
                var equipo = new Models.Equipo
                {
                    EquipoId = row.Team.Id,
                    Nombre = row.Team.Name,
                    Slug = row.Team.Slug,
                    NombreCorto = row.Team.ShortName,
                    CodigoCorto = row.Team.NameCode,
                    ColorPrimario = row.Team.TeamColors?.Primary,
                    ColorSecundario = row.Team.TeamColors?.Secondary
                };

                await _equipoRepository.UpsertAsync(equipo);
                equiposSincronizados++;
            }

            return Ok(new { mensaje = $"{equiposSincronizados} equipos sincronizados correctamente." });
        }
    }
}