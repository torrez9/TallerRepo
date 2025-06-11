using Microsoft.EntityFrameworkCore;
using TallerWEBAPI.Models;

namespace TallerWEBAPI.Services
{
    public class ClienteService
    {
        private readonly MotosTuning3Context _context;

        public ClienteService(MotosTuning3Context context)
        {
            _context = context;
        }

        //
        public async Task<Cliente?> ObtenerClientePorIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<IEnumerable<Cliente>> ObtenerClientesAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        //
        public async Task<Cliente?> ObtenerClientePorUsuarioAsync(string usuario)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Usuario == usuario);
        }
        //
        public async Task<bool> ActualizarPerfilAsync(int idCliente, Cliente cliente, bool actualizarContraseña = false)
        {
            var clienteExistente = await _context.Clientes.FindAsync(idCliente);
            if (clienteExistente == null)
                return false;

            // Actualizar solo campos permitidos
            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Apellido = cliente.Apellido;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Direccion = cliente.Direccion;

            if (actualizarContraseña && !string.IsNullOrEmpty(cliente.Contraseña))
            {
                clienteExistente.Contraseña = cliente.Contraseña; // Debería estar hasheada
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerificarRelacionesAsync(int idCliente)
        {
            return await _context.Citas.AnyAsync(c => c.IdCliente == idCliente) ||
                   await _context.Facturas.AnyAsync(f => f.IdCliente == idCliente) ||
                   await _context.Motos.AnyAsync(m => m.IdCliente == idCliente);
        }

        public async Task<Cliente> CrearClienteAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> ActualizarClienteAsync(int id, Cliente cliente)
        {
            var clienteExistente = await _context.Clientes.FindAsync(id);
            if (clienteExistente == null)
                return false;

            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Correo = cliente.Correo;
            clienteExistente.Direccion = cliente.Direccion;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarClienteAsync(int id, bool eliminarRelaciones = false)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Cita)
                .Include(c => c.Facturas)
                .Include(c => c.Motos)
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null)
                return false;

            if (eliminarRelaciones)
            {
                _context.Citas.RemoveRange(cliente.Cita);
                _context.Facturas.RemoveRange(cliente.Facturas);
                _context.Motos.RemoveRange(cliente.Motos);
            }
            else if (cliente.Cita.Any() || cliente.Facturas.Any() || cliente.Motos.Any())
            {
                return false;
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}