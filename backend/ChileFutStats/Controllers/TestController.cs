using ChileFutStats.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChileFutStats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public TestController(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [HttpGet("db")]
        public IActionResult TestDbConnection()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                connection.Open();
                return Ok(new { mensaje = "Conexión exitosa a Oracle" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
