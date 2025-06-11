using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerWEBAPI.Models;

namespace TallerWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticasCitasController : ControllerBase
    {
        private readonly MotosTuning3Context _context;

        public EstadisticasCitasController(MotosTuning3Context context)
        {
            _context = context;
        }

        // GET: api/EstadisticasCitas/ResumenEstados
        [HttpGet("ResumenEstados")]
        public async Task<ActionResult<object>> GetResumenEstadosCitas()
        {
            var resumen = await _context.Citas
                .GroupBy(c => c.Estado)
                .Select(g => new
                {
                    Estado = g.Key,
                    Cantidad = g.Count()
                })
                .ToListAsync();

            return Ok(new
            {
                Labels = resumen.Select(r => r.Estado).ToArray(),
                Datos = resumen.Select(r => r.Cantidad).ToArray()
            });
        }

        // GET: api/EstadisticasCitas/CitasPorMes
        [HttpGet("CitasPorMes")]
        public async Task<ActionResult<object>> GetCitasPorMes()
        {
            var citasPorMes = await _context.Citas
                .GroupBy(c => new { c.FechaCita.Year, c.FechaCita.Month })
                .Select(g => new
                {
                    Año = g.Key.Year,
                    Mes = g.Key.Month,
                    Cantidad = g.Count()
                })
                .OrderBy(x => x.Año)
                .ThenBy(x => x.Mes)
                .ToListAsync();

            var meses = citasPorMes.Select(x => $"{x.Año}-{x.Mes.ToString().PadLeft(2, '0')}").ToArray();
            var cantidades = citasPorMes.Select(x => x.Cantidad).ToArray();

            return Ok(new
            {
                Labels = meses,
                Datos = cantidades
            });
        }

        // GET: api/EstadisticasCitas/CitasPorCliente
        [HttpGet("CitasPorCliente")]
        public async Task<ActionResult<object>> GetCitasPorCliente()
        {
            var citasPorCliente = await _context.Citas
                .Include(c => c.IdClienteNavigation)
                .GroupBy(c => new { c.IdCliente, c.IdClienteNavigation.Nombre })
                .Select(g => new
                {
                    ClienteId = g.Key.IdCliente,
                    ClienteNombre = g.Key.Nombre,
                    Cantidad = g.Count()
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(10) // Solo los 10 clientes con más citas
                .ToListAsync();

            return Ok(new
            {
                Labels = citasPorCliente.Select(x => x.ClienteNombre).ToArray(),
                Datos = citasPorCliente.Select(x => x.Cantidad).ToArray()
            });
        }
    }
}
