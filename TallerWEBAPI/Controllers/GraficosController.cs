using Microsoft.AspNetCore.Mvc;
using TallerWEBAPI.Models;
using TallerWEBAPI.Services;
using System.Globalization;

namespace TallerWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraficosController : ControllerBase
    {
        private readonly CitaService _citaService;
        private readonly ClienteService _clienteService;

        public GraficosController(CitaService citaService, ClienteService clienteService)
        {
            _citaService = citaService;
            _clienteService = clienteService;
        }

        // GET: api/Graficos/EstadosCitas
        [HttpGet("EstadosCitas")]
        public async Task<ActionResult> GetEstadosCitas()
        {
            var citas = await _citaService.ObtenerCitasAsync();
            var resumen = citas
                .GroupBy(c => c.Estado)
                .Select(g => new { Estado = g.Key, Total = g.Count() })
                .ToList();

            return Ok(resumen);
        }

        // GET: api/Graficos/CitasPorMes
        [HttpGet("CitasPorMes")]
        public async Task<ActionResult> GetCitasPorMes()
        {
            var citas = await _citaService.ObtenerCitasAsync();
            var ahora = DateTime.Now;
            var ultimos6Meses = citas
                .Where(c => c.FechaCita >= ahora.AddMonths(-6))
                .GroupBy(c => new { c.FechaCita.Year, c.FechaCita.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month) + " " + g.Key.Year,
                    Cantidad = g.Count()
                })
                .ToList();

            return Ok(ultimos6Meses);
        }

        // GET: api/Graficos/TopClientes
        [HttpGet("TopClientes")]
        public async Task<ActionResult> GetTopClientes()
        {
            var citas = await _citaService.ObtenerCitasAsync();
            var clientes = await _clienteService.ObtenerClientesAsync();

            var topClientes = citas
                .GroupBy(c => c.IdCliente)
                .Select(g => new
                {
                    IdCliente = g.Key,
                    Nombre = clientes.FirstOrDefault(cli => cli.IdCliente == g.Key)?.Nombre ?? "Desconocido",
                    TotalCitas = g.Count()
                })
                .OrderByDescending(x => x.TotalCitas)
                .Take(10)
                .ToList();

            return Ok(topClientes);
        }
    }
}
