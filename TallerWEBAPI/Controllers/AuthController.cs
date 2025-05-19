using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TallerWEBAPI.Models;

namespace TallerWEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly MotosTuningContext _context;

        public AuthController(AuthService authService, IConfiguration configuration, EmailService emailService, MotosTuningContext context)
        {
            _authService = authService;
            _configuration = configuration;
            _emailService = emailService;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = _authService.ValidarCredenciales(request.Correo, request.Contraseña);
            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas" });

            return Ok(new
            {
                mensaje = "Login exitoso",
                usuario = new
                {
                    usuario.IdUsuario,
                    usuario.Nombre,
                    usuario.Correo,
                    usuario.IdRol,
                    Rol = usuario.IdRolNavigation?.NombreRol
                },
                token = _authService.GenerarTokenJWT(usuario, _configuration)
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UsuarioRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = int.Parse(request.Telefono),
                Correo = request.Correo,
                Usuario1 = request.NombreUsuario,
                Contraseña = BCrypt.Net.BCrypt.HashPassword(request.Contraseña),
                IdRol = 2
            };

            var exito = _authService.RegistrarUsuario(usuario);

            if (!exito)
                return Conflict(new { mensaje = "Ya existe un usuario con ese correo." });

            return Ok(new
            {
                mensaje = "Usuario registrado correctamente.",
                usuario = new
                {
                    usuario.IdUsuario,
                    usuario.Nombre,
                    usuario.Correo,
                    usuario.IdRol
                }
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _authService.ResetPassword(request.Email, _emailService);
                return result ? Ok(new { mensaje = "Se ha enviado una nueva contraseña a tu correo electrónico" })
                             : BadRequest(new { mensaje = "No se encontró un usuario con ese correo electrónico" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error al restablecer contraseña: {ex.Message}" });
            }
        }

        [HttpGet("user/{id}")]
        public IActionResult GetUserData(int id)
        {
            var usuario = _context.Usuarios.Include(u => u.IdRolNavigation).FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            return Ok(new
            {
                usuario.IdUsuario,
                usuario.Nombre,
                usuario.Apellido,
                usuario.Telefono,
                usuario.Correo,
                usuario.Usuario1,
                Rol = usuario.IdRolNavigation?.NombreRol
            });
        }

        [HttpPut("user/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuario = _context.Usuarios.Find(id);
                if (usuario == null)
                    return NotFound(new { mensaje = "Usuario no encontrado" });

                if (!string.IsNullOrEmpty(request.NombreUsuario) &&
                    _context.Usuarios.Any(u => u.Usuario1 == request.NombreUsuario && u.IdUsuario != id))
                {
                    return BadRequest(new { mensaje = "El nombre de usuario ya está en uso" });
                }

                usuario.Nombre = request.Nombre ?? usuario.Nombre;
                usuario.Apellido = request.Apellido ?? usuario.Apellido;
                usuario.Telefono = request.Telefono ?? usuario.Telefono;
                usuario.Usuario1 = request.NombreUsuario ?? usuario.Usuario1;

                _context.SaveChanges();

                return Ok(new
                {
                    mensaje = "Usuario actualizado correctamente",
                    usuario = new
                    {
                        usuario.IdUsuario,
                        usuario.Nombre,
                        usuario.Apellido,
                        usuario.Telefono,
                        usuario.Correo,
                        usuario.Usuario1
                    }
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensaje = $"Error al actualizar usuario: {ex.InnerException?.Message ?? ex.Message}" });
            }
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.NuevaContraseña != request.ConfirmarContraseña)
                return BadRequest(new { mensaje = "Las contraseñas no coinciden" });

            var usuario = _context.Usuarios.Find(request.UsuarioId);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            if (!BCrypt.Net.BCrypt.Verify(request.ContraseñaActual, usuario.Contraseña))
                return BadRequest(new { mensaje = "Contraseña actual incorrecta" });

            if (BCrypt.Net.BCrypt.Verify(request.NuevaContraseña, usuario.Contraseña))
                return BadRequest(new { mensaje = "La nueva contraseña debe ser diferente a la actual" });

            usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(request.NuevaContraseña);

            try
            {
                _context.SaveChanges();
                return Ok(new { mensaje = "Contraseña cambiada exitosamente" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensaje = $"Error al cambiar contraseña: {ex.InnerException?.Message ?? ex.Message}" });
            }
        }

        // Modelos de request con validaciones
        public class LoginRequest
        {
            [Required]
            [EmailAddress]
            public string Correo { get; set; }

            [Required]
            public string Contraseña { get; set; }
        }

        public class UsuarioRequest
        {
            [Required]
            [MinLength(2)]
            public string Nombre { get; set; }

            [Required]
            [MinLength(2)]
            public string Apellido { get; set; }

            [Required]
            [Phone]
            public string Telefono { get; set; }

            [Required]
            [EmailAddress]
            public string Correo { get; set; }

            [Required]
            [MinLength(4)]
            public string NombreUsuario { get; set; }

            [Required]
            [MinLength(6)]
            public string Contraseña { get; set; }
        }

        public class ResetPasswordRequest
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public class UpdateUserRequest
        {
            [MinLength(2)]
            public string Nombre { get; set; }

            [MinLength(2)]
            public string Apellido { get; set; }

            [Range(1000000, 9999999999)]
            public int? Telefono { get; set; }

            [MinLength(4)]
            public string NombreUsuario { get; set; }
        }

        public class ChangePasswordRequest
        {
            [Required]
            public int UsuarioId { get; set; }

            [Required]
            public string ContraseñaActual { get; set; }

            [Required]
            [MinLength(6)]
            public string NuevaContraseña { get; set; }

            [Required]
            [Compare("NuevaContraseña", ErrorMessage = "Las contraseñas no coinciden")]
            public string ConfirmarContraseña { get; set; }
        }
    }
}
