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
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            AuthService authService,
            IConfiguration configuration,
            EmailService emailService,
            MotosTuningContext context,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _configuration = configuration;
            _emailService = emailService;
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var cliente = await _authService.ValidarCredenciales(request.Correo, request.Contraseña);
                if (cliente == null)
                    return Unauthorized(new { mensaje = "Credenciales inválidas" });

                var token = _authService.GenerarTokenJWT(cliente, _configuration);

                _logger.LogInformation($"Login exitoso para el usuario: {cliente.Correo}");

                return Ok(new
                {
                    mensaje = "Login exitoso",
                    cliente = new
                    {
                        cliente.IdCliente,
                        cliente.Nombre,
                        cliente.Correo,
                        cliente.Usuario,
                        cliente.Apellido,
                        cliente.Telefono,
                        cliente.Direccion
                    },
                    token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el proceso de login");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ClienteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (request.Contraseña != request.ConfirmarContraseña)
                    return BadRequest(new { mensaje = "Las contraseñas no coinciden" });

                var cliente = new Cliente
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Telefono = request.Telefono,
                    Correo = request.Correo,
                    Usuario = request.NombreUsuario,
                    Contraseña = request.Contraseña, // Se hashea en el servicio
                    Direccion = request.Direccion
                };

                var resultado = await _authService.RegistrarCliente(cliente);

                if (!resultado.Exito)
                    return Conflict(new
                    {
                        mensaje = resultado.MensajeError ?? "Error al registrar el cliente"
                    });

                _logger.LogInformation($"Nuevo cliente registrado: {cliente.Correo}");

                return Ok(new
                {
                    mensaje = "Cliente registrado correctamente.",
                    cliente = new
                    {
                        IdCliente = cliente.IdCliente,
                        cliente.Nombre,
                        cliente.Correo,
                        cliente.Usuario,
                        cliente.Apellido,
                        cliente.Telefono,
                        cliente.Direccion
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en el registro de cliente: {request.Correo}");
                return StatusCode(500, new
                {
                    mensaje = "Error interno del servidor",
                    detalle = ex.Message
                });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.ResetPassword(request.Email, _emailService);

                if (!result.Exito)
                    return BadRequest(new { mensaje = result.MensajeError });

                _logger.LogInformation($"Contraseña reseteada para: {request.Email}");

                return Ok(new { mensaje = "Se ha enviado una nueva contraseña a tu correo electrónico" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al restablecer contraseña para: {request.Email}");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpGet("cliente/{id}")]
        [Authorize]
        public async Task<IActionResult> GetClienteData(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id);
                if (cliente == null)
                    return NotFound(new { mensaje = "Cliente no encontrado" });

                return Ok(new
                {
                    cliente.IdCliente,
                    cliente.Nombre,
                    cliente.Apellido,
                    cliente.Telefono,
                    cliente.Correo,
                    cliente.Usuario,
                    cliente.Direccion
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener datos del cliente ID: {id}");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPut("cliente/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] UpdateClienteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                    return NotFound(new { mensaje = "Cliente no encontrado" });

                if (!string.IsNullOrEmpty(request.NombreUsuario) &&
                    await _context.Clientes.AnyAsync(c => c.Usuario == request.NombreUsuario && c.IdCliente != id))
                {
                    return BadRequest(new { mensaje = "El nombre de usuario ya está en uso" });
                }

                cliente.Nombre = request.Nombre ?? cliente.Nombre;
                cliente.Apellido = request.Apellido ?? cliente.Apellido;
                cliente.Telefono = request.Telefono ?? cliente.Telefono;
                cliente.Usuario = request.NombreUsuario ?? cliente.Usuario;
                cliente.Direccion = request.Direccion ?? cliente.Direccion;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Cliente actualizado ID: {id}");

                return Ok(new
                {
                    mensaje = "Cliente actualizado correctamente",
                    cliente = new
                    {
                        cliente.IdCliente,
                        cliente.Nombre,
                        cliente.Apellido,
                        cliente.Telefono,
                        cliente.Correo,
                        cliente.Usuario,
                        cliente.Direccion
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar cliente ID: {id}");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] AuthChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (request.NuevaContraseña != request.ConfirmarContraseña)
                    return BadRequest(new { mensaje = "Las contraseñas no coinciden" });

                var cliente = await _context.Clientes.FindAsync(request.ClienteId);
                if (cliente == null)
                    return NotFound(new { mensaje = "Cliente no encontrado" });

                if (!BCrypt.Net.BCrypt.Verify(request.ContraseñaActual, cliente.Contraseña))
                    return BadRequest(new { mensaje = "Contraseña actual incorrecta" });

                if (BCrypt.Net.BCrypt.Verify(request.NuevaContraseña, cliente.Contraseña))
                    return BadRequest(new { mensaje = "La nueva contraseña debe ser diferente a la actual" });

                cliente.Contraseña = BCrypt.Net.BCrypt.HashPassword(request.NuevaContraseña);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Contraseña cambiada para cliente ID: {request.ClienteId}");

                return Ok(new { mensaje = "Contraseña cambiada exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cambiar contraseña para cliente ID: {request.ClienteId}");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        // Modelos de request con validaciones
        public class LoginRequest
        {
            [Required(ErrorMessage = "El correo es requerido")]
            [EmailAddress(ErrorMessage = "Correo electrónico no válido")]
            public string Correo { get; set; }

            [Required(ErrorMessage = "La contraseña es requerida")]
            public string Contraseña { get; set; }
        }

        public class ClienteRequest
        {
            [Required(ErrorMessage = "El nombre es requerido")]
            [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "El apellido es requerido")]
            [MinLength(2, ErrorMessage = "El apellido debe tener al menos 2 caracteres")]
            public string Apellido { get; set; }

            [Required(ErrorMessage = "El teléfono es requerido")]
            [Phone(ErrorMessage = "Teléfono no válido")]
            public string Telefono { get; set; }

            [Required(ErrorMessage = "El correo es requerido")]
            [EmailAddress(ErrorMessage = "Correo electrónico no válido")]
            public string Correo { get; set; }

            [Required(ErrorMessage = "El nombre de usuario es requerido")]
            [MinLength(4, ErrorMessage = "El nombre de usuario debe tener al menos 4 caracteres")]
            public string NombreUsuario { get; set; }

            [Required(ErrorMessage = "La contraseña es requerida")]
            [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
            public string Contraseña { get; set; }

            [Required(ErrorMessage = "Debes confirmar la contraseña")]
            [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden")]
            public string ConfirmarContraseña { get; set; }

            public string? Direccion { get; set; }
        }

        public class ResetPasswordRequest
        {
            [Required(ErrorMessage = "El correo es requerido")]
            [EmailAddress(ErrorMessage = "Correo electrónico no válido")]
            public string Email { get; set; }
        }

        public class UpdateClienteRequest
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

        public class AuthChangePasswordRequest
        {
            [Required(ErrorMessage = "El ID de cliente es requerido")]
            public int ClienteId { get; set; }

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