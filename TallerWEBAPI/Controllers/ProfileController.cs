using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TallerWEBAPI.Models;
using TallerWEBAPI.Services;

namespace TallerWEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileService _profileService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            ProfileService profileService,
            ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var clienteId = GetCurrentUserId();
                if (clienteId == null)
                    return Unauthorized();

                var cliente = await _profileService.GetProfileData(clienteId.Value);
                if (cliente == null)
                    return NotFound(new { mensaje = "Perfil no encontrado" });

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var clienteId = GetCurrentUserId();
                if (clienteId == null)
                    return Unauthorized();

                var result = await _profileService.UpdateProfile(clienteId.Value, request);
                if (!result)
                    return BadRequest(new { mensaje = "Error al actualizar el perfil" });

                return Ok(new { mensaje = "Perfil actualizado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar perfil");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var clienteId = GetCurrentUserId();
                if (clienteId == null)
                    return Unauthorized();

                var result = await _profileService.ChangePassword(clienteId.Value, request);
                if (!result)
                    return BadRequest(new { mensaje = "Error al cambiar la contraseña" });

                return Ok(new { mensaje = "Contraseña cambiada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar contraseña");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return null;

            return int.Parse(userIdClaim);
        }

        // Modelos de request
        public class UpdateProfileRequest
        {
            [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
            public string? Nombre { get; set; }

            [MinLength(2, ErrorMessage = "El apellido debe tener al menos 2 caracteres")]
            public string? Apellido { get; set; }

            [Phone(ErrorMessage = "Teléfono no válido")]
            public string? Telefono { get; set; }

            [MinLength(4, ErrorMessage = "El nombre de usuario debe tener al menos 4 caracteres")]
            public string? NombreUsuario { get; set; }

            public string? Direccion { get; set; }
        }

        public class ChangePasswordRequest
        {
            [Required(ErrorMessage = "La contraseña actual es requerida")]
            public string ContraseñaActual { get; set; }

            [Required(ErrorMessage = "La nueva contraseña es requerida")]
            [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
            public string NuevaContraseña { get; set; }

            [Required(ErrorMessage = "Debes confirmar la nueva contraseña")]
            [Compare("NuevaContraseña", ErrorMessage = "Las contraseñas no coinciden")]
            public string ConfirmarContraseña { get; set; }
        }
    }
}