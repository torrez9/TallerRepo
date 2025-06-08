using Microsoft.EntityFrameworkCore;
using TallerWEBAPI.Models;

namespace TallerWEBAPI.Services
{
    public class MisCitasService
    {
        private readonly MotosTuningContext _context;

        public MisCitasService(MotosTuningContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cita>> ObtenerMisCitasAsync(int idCliente)
        {
            return await _context.Citas
                .Where(c => c.IdCliente == idCliente)
                .Include(c => c.IdClienteNavigation)
                .OrderByDescending(c => c.FechaCita)
                .ThenBy(c => c.Hora)
                .ToListAsync();
        }

        public async Task<Cita?> ObtenerMiCitaPorIdAsync(int idCita, int idCliente)
        {
            return await _context.Citas
                .Include(c => c.IdClienteNavigation)
                .FirstOrDefaultAsync(c => c.IdCita == idCita && c.IdCliente == idCliente);
        }

        public async Task<Cita> CrearMiCitaAsync(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return await _context.Citas
                .Include(c => c.IdClienteNavigation)
                .FirstOrDefaultAsync(c => c.IdCita == cita.IdCita);
        }

        public async Task<bool> ActualizarMiCitaAsync(Cita cita)
        {
            var citaExistente = await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == cita.IdCita && c.IdCliente == cita.IdCliente);

            if (citaExistente == null)
                return false;

            citaExistente.FechaCita = cita.FechaCita;
            citaExistente.Estado = cita.Estado;
            citaExistente.Descripcion = cita.Descripcion;
            citaExistente.Hora = cita.Hora;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarMiCitaAsync(int idCita, int idCliente)
        {
            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == idCita && c.IdCliente == idCliente);

            if (cita == null)
                return false;

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}