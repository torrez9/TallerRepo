using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TallerWEBAPI.Services;

namespace TallerWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificacionesController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Envía notificaciones para todas las citas del día actual
        /// </summary>
        [HttpPost("notificar-citas-hoy")]
        public async Task<IActionResult> NotificarCitasDelDia()
        {
            try
            {
                await _notificationService.NotificarCitasDelDia();
                return Ok(new { success = true, message = "Notificaciones enviadas correctamente" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error al enviar notificaciones: {ex.Message}" });
            }
        }

        /// <summary>
        /// Envía una notificación para una cita específica
        /// </summary>
        /// <param name="citaId">ID de la cita a notificar</param>
        [HttpPost("notificar-cita/{citaId}")]
        public async Task<IActionResult> NotificarCitaIndividual(int citaId)
        {
            try
            {
                // En una implementación real, aquí deberías obtener la cita y el cliente
                // de tus servicios de base de datos
                // Por simplicidad, asumimos que el NotificationService ya maneja esto

                // Esta implementación asume que el servicio puede manejar la notificación
                // individual con solo el ID de la cita
                // Si no es así, necesitarías modificar el INotificationService para incluir
                // un método que acepte solo el ID de la cita

                return BadRequest(new { success = false, message = "Implementación pendiente: Necesitas obtener la cita y el cliente primero" });

                // Implementación alternativa si modificas la interfaz:
                // var cita = await _citaService.ObtenerCitaPorIdAsync(citaId);
                // var cliente = await _clienteService.ObtenerClientePorIdAsync(cita.IdCliente);
                // await _notificationService.EnviarNotificacionCita(cita, cliente);
                // return Ok(new { success = true, message = $"Notificación enviada a {cliente.Nombre}" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error al enviar notificación: {ex.Message}" });
            }
        }
    }
}