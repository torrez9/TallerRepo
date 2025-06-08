using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TallerWEBAPI.Models;
using TallerWEBAPI.Services;

namespace TallerWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly CitaService _citaService;
        private readonly INotificationService _notificationService;

        public CitasController(CitaService citaService, INotificationService notificationService)
        {
            _citaService = citaService;
            _notificationService = notificationService;
        }


        //citas hoy
        [HttpGet("Hoy")]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitasDeHoy()
        {
            try
            {
                var citas = await _citaService.ObtenerCitasDeHoyAsync();
                return Ok(citas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener las citas de hoy.",
                    detalles = ex.Message
                });
            }
        }

        //notificaciones hoy
        [HttpPost("NotificarCitasHoy")]
        public async Task<IActionResult> NotificarCitasDelDia()
        {
            try
            {
                await _notificationService.NotificarCitasDelDia();
                return Ok(new { mensaje = "Notificaciones enviadas correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al enviar notificaciones",
                    detalles = ex.Message
                });
            }
        }

        // GET: api/Citas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitas()
        {
            try
            {
                var citas = await _citaService.ObtenerCitasAsync();
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

        // GET: api/Citas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetCita(int id)
        {
            try
            {
                var cita = await _citaService.ObtenerCitaPorIdAsync(id);

                if (cita == null)
                {
                    return NotFound($"No se encontró la cita con ID {id}");
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

        // GET: api/Citas/Cliente/5
        [HttpGet("Cliente/{idCliente}")]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitasPorCliente(int idCliente)
        {
            try
            {
                var citas = await _citaService.ObtenerCitasPorClienteAsync(idCliente);
                return Ok(citas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener las citas del cliente.",
                    detalles = ex.Message
                });
            }
        }

        // POST: api/Citas
        [HttpPost]
        public async Task<ActionResult<Cita>> PostCita([FromBody] Cita cita)
        {
            try
            {
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

                if (!await _citaService.ClienteExisteAsync(cita.IdCliente))
                {
                    return BadRequest("El cliente especificado no existe");
                }

                var nuevaCita = await _citaService.CrearCitaAsync(cita);
                return CreatedAtAction(nameof(GetCita), new { id = nuevaCita.IdCita }, nuevaCita);
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

        // PUT: api/Citas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, [FromBody] Cita cita)
        {
            try
            {
                if (id != cita.IdCita)
                {
                    return BadRequest("El ID de la cita no coincide con el ID en la URL");
                }

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

                if (!await _citaService.ClienteExisteAsync(cita.IdCliente))
                {
                    return BadRequest("El cliente especificado no existe");
                }

                var actualizacionExitosa = await _citaService.ActualizarCitaAsync(cita);

                if (!actualizacionExitosa)
                {
                    return NotFound($"No se encontró la cita con ID {id}");
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

        // DELETE: api/Citas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            try
            {
                var eliminacionExitosa = await _citaService.EliminarCitaAsync(id);

                if (!eliminacionExitosa)
                {
                    return NotFound($"No se encontró la cita con ID {id}");
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

        // DELETE: api/Citas/Cliente/5
        [HttpDelete("Cliente/{idCliente}")]
        public async Task<IActionResult> DeleteCitasPorCliente(int idCliente)
        {
            try
            {
                var eliminacionExitosa = await _citaService.EliminarCitasPorClienteAsync(idCliente);

                if (!eliminacionExitosa)
                {
                    return NotFound($"No se encontraron citas para el cliente con ID {idCliente}");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al eliminar las citas del cliente.",
                    detalles = ex.Message
                });
            }
        }

        [HttpGet("MisCitas")]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitasPorUsuario()
        {
            try
            {
                // Obtener el ID del cliente desde el token
                var clienteIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (clienteIdClaim == null || !int.TryParse(clienteIdClaim.Value, out int idCliente))
                {
                    return Unauthorized("Token inválido o no contiene información del cliente.");
                }

                var citas = await _citaService.ObtenerCitasPorClienteAsync(idCliente);
                return Ok(citas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al obtener las citas del cliente.",
                    detalles = ex.Message
                });
            }
        }
    }
}