using ChileFutStats.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ChileFutStats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipoController : ControllerBase
    {
        private readonly EquipoRepository _equipoRepository;

        public EquipoController(EquipoRepository equipoRepository)
        {
            _equipoRepository = equipoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var equipos = await _equipoRepository.GetAllAsync();
            return Ok(equipos);
        }
    }
}