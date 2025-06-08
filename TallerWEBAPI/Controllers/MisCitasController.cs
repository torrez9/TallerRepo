using Microsoft.AspNetCore.Mvc;
using TallerWEBAPI.Models;
using TallerWEBAPI.Services;

namespace TallerWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MisCitasController : ControllerBase
    {
        private readonly MisCitasService _misCitasService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MisCitasController(MisCitasService misCitasService, IHttpContextAccessor httpContextAccessor)
        {
            _misCitasService = misCitasService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/MisCitas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cita>>> GetMisCitas()
        {
            try
            {
                // Obtener el ID del usuario autenticado (ajusta según tu sistema de autenticación)
                var userId = GetAuthenticatedUserId();
                if (userId == null)
                {
                    return Unauthorized("Usuario no autenticado");
                }

                var citas = await _misCitasService.ObtenerMisCitasAsync(userId.Value);
                return Ok(citas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener las citas.",
                    detalles = ex.Message
                });
            }
        }

        // GET: api/MisCitas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetMiCita(int id)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                if (userId == null)
                {
                    return Unauthorized("Usuario no autenticado");
                }

                var cita = await _misCitasService.ObtenerMiCitaPorIdAsync(id, userId.Value);

                if (cita == null)
                {
                    return NotFound($"No se encontró la cita con ID {id} para este usuario");
                }

                return Ok(cita);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener la cita.",
                    detalles = ex.Message
                });
            }
        }

        // POST: api/MisCitas
        [HttpPost]
        public async Task<ActionResult<Cita>> PostMiCita([FromBody] Cita cita)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                if (userId == null)
                {
                    return Unauthorized("Usuario no autenticado");
                }

                // Validaciones básicas
                if (cita.FechaCita == default)
                {
                    return BadRequest("La fecha de la cita es requerida");
                }

                if (cita.Hora == null || cita.Hora == default(TimeOnly))
                {
                    return BadRequest("La hora de la cita es requerida");
                }

                if (string.IsNullOrEmpty(cita.Descripcion))
                {
                    return BadRequest("La descripción de la cita es requerida");
                }

                // Asignar el cliente autenticado
                cita.IdCliente = userId.Value;

                var nuevaCita = await _misCitasService.CrearMiCitaAsync(cita);
                return CreatedAtAction(nameof(GetMiCita), new { id = nuevaCita.IdCita }, nuevaCita);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al crear la cita.",
                    detalles = ex.Message
                });
            }
        }

        // PUT: api/MisCitas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMiCita(int id, [FromBody] Cita cita)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                if (userId == null)
                {
                    return Unauthorized("Usuario no autenticado");
                }

                if (id != cita.IdCita)
                {
                    return BadRequest("El ID de la cita no coincide con el ID en la URL");
                }

                // Validaciones básicas
                if (cita.FechaCita == default)
                {
                    return BadRequest("La fecha de la cita es requerida");
                }

                if (cita.Hora == null || cita.Hora == default(TimeOnly))
                {
                    return BadRequest("La hora de la cita es requerida");
                }

                if (string.IsNullOrEmpty(cita.Descripcion))
                {
                    return BadRequest("La descripción de la cita es requerida");
                }

                // Verificar que la cita pertenece al usuario
                if (cita.IdCliente != userId.Value)
                {
                    return Forbid("No tienes permiso para modificar esta cita");
                }

                var actualizacionExitosa = await _misCitasService.ActualizarMiCitaAsync(cita);

                if (!actualizacionExitosa)
                {
                    return NotFound($"No se encontró la cita con ID {id} para este usuario");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al actualizar la cita.",
                    detalles = ex.Message
                });
            }
        }

        // DELETE: api/MisCitas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMiCita(int id)
        {
            try
            {
                var userId = GetAuthenticatedUserId();
                if (userId == null)
                {
                    return Unauthorized("Usuario no autenticado");
                }

                var eliminacionExitosa = await _misCitasService.EliminarMiCitaAsync(id, userId.Value);

                if (!eliminacionExitosa)
                {
                    return NotFound($"No se encontró la cita con ID {id} para este usuario");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al eliminar la cita.",
                    detalles = ex.Message
                });
            }
        }

        private int? GetAuthenticatedUserId()
        {
            // Implementa la lógica para obtener el ID del usuario autenticado
            // Esto depende de tu sistema de autenticación
            // Ejemplo básico:
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst("userId");
            if (claim != null && int.TryParse(claim.Value, out var userId))
            {
                return userId;
            }
            return null;
        }
    }
}