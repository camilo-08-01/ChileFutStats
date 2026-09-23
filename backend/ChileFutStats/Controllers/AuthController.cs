using ChileFutStats.DTOs;
using ChileFutStats.Models;
using ChileFutStats.Repositories;
using ChileFutStats.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChileFutStats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly TokenService _tokenService;

        public AuthController(UsuarioRepository usuarioRepository, TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(RegistroRequest request)
        {
            var usuarioExistente = await _usuarioRepository.GetByNombreUsuarioAsync(request.NombreUsuario);
            if (usuarioExistente != null)
            {
                return Conflict(new { error = "Ese nombre de usuario ya está en uso." });
            }

            if (await _usuarioRepository.ExisteEmailAsync(request.Email))
            {
                return Conflict(new { error = "Ese email ya está registrado." });
            }

            var nuevoUsuario = new Usuario
            {
                NombreUsuario = request.NombreUsuario,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            var nuevoId = await _usuarioRepository.CreateAsync(nuevoUsuario);
            nuevoUsuario.UsuarioId = nuevoId;

            var token = _tokenService.GenerarToken(nuevoUsuario);

            return Ok(new AuthResponse { Token = token, NombreUsuario = nuevoUsuario.NombreUsuario });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(request.NombreUsuario);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            {
                return Unauthorized(new { error = "Usuario o contraseña incorrectos." });
            }

            var token = _tokenService.GenerarToken(usuario);

            return Ok(new AuthResponse { Token = token, NombreUsuario = usuario.NombreUsuario });
        }
    }
}