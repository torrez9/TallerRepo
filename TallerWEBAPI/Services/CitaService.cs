using Microsoft.EntityFrameworkCore;
using TallerWEBAPI.Models;

namespace TallerWEBAPI.Services
{
    public class CitaService
    {
        private readonly MotosTuningContext _context;

        public CitaService(MotosTuningContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cita>> ObtenerCitasAsync()
        {
            return await _context.Citas
                .Include(c => c.IdClienteNavigation)
                .OrderByDescending(c => c.FechaCita)
                .ThenBy(c => c.Hora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> ObtenerCitasPorClienteAsync(int idCliente)
        {
            return await _context.Citas
                .Where(c => c.IdCliente == idCliente)
                .Include(c => c.IdClienteNavigation)
                .OrderByDescending(c => c.FechaCita)
                .ThenBy(c => c.Hora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cita>> ObtenerCitasDeHoyAsync()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return await _context.Citas
                .Where(c => c.FechaCita == hoy)
                .Include(c => c.IdClienteNavigation)
                .OrderBy(c => c.Hora)
                .ToListAsync();
        }

        public async Task<Cita?> ObtenerCitaPorIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.IdClienteNavigation)
                .FirstOrDefaultAsync(c => c.IdCita == id);
        }

        public async Task<Cita> CrearCitaAsync(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return await _context.Citas
                .Include(c => c.IdClienteNavigation)
                .FirstOrDefaultAsync(c => c.IdCita == cita.IdCita);
        }

        public async Task<bool> ActualizarCitaAsync(Cita cita)
        {
            var citaExistente = await _context.Citas.FindAsync(cita.IdCita);
            if (citaExistente == null)
                return false;

            citaExistente.IdCliente = cita.IdCliente;
            citaExistente.FechaCita = cita.FechaCita;
            citaExistente.Estado = cita.Estado;
            citaExistente.Descripcion = cita.Descripcion;
            citaExistente.Hora = cita.Hora;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarCitaAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
                return false;

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarCitasPorClienteAsync(int idCliente)
        {
            var citas = await _context.Citas
                .Where(c => c.IdCliente == idCliente)
                .ToListAsync();

            if (!citas.Any())
                return false;

            _context.Citas.RemoveRange(citas);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClienteExisteAsync(int idCliente)
        {
            return await _context.Clientes.AnyAsync(c => c.IdCliente == idCliente);
        }
    }
}