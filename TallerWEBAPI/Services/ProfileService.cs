using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using TallerWEBAPI.Controllers;
using TallerWEBAPI.Models;

public class ProfileService
{
    private readonly MotosTuning3Context _context;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(MotosTuning3Context context, ILogger<ProfileService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Cliente?> GetProfileData(int clienteId)
    {
        try
        {
            return await _context.Clientes
                .Where(c => c.IdCliente == clienteId)
                .Select(c => new Cliente
                {
                    IdCliente = c.IdCliente,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Telefono = c.Telefono,
                    Correo = c.Correo,
                    Usuario = c.Usuario,
                    Direccion = c.Direccion
                    // Contraseña omitida intencionalmente
                })
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener perfil del cliente ID: {clienteId}");
            throw;
        }
    }

    public async Task<bool> UpdateProfile(int clienteId, ProfileController.UpdateProfileRequest request)
    {
        try
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
                return false;

            // Verificar si el nombre de usuario ya está en uso por otro cliente
            if (!string.IsNullOrEmpty(request.NombreUsuario) &&
                await _context.Clientes.AnyAsync(c => c.Usuario == request.NombreUsuario && c.IdCliente != clienteId))
            {
                return false;
            }

            // Actualizar solo los campos que se proporcionaron
            cliente.Nombre = request.Nombre ?? cliente.Nombre;
            cliente.Apellido = request.Apellido ?? cliente.Apellido;
            cliente.Telefono = request.Telefono ?? cliente.Telefono;
            cliente.Usuario = request.NombreUsuario ?? cliente.Usuario;
            cliente.Direccion = request.Direccion ?? cliente.Direccion;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar perfil del cliente ID: {clienteId}");
            throw;
        }
    }

    public async Task<bool> ChangePassword(int clienteId, ProfileController.ChangePasswordRequest request)
    {
        try
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
                return false;

            // Verificar contraseña actual
            if (!BCrypt.Net.BCrypt.Verify(request.ContraseñaActual, cliente.Contraseña))
                return false;

            // Verificar que la nueva contraseña sea diferente
            if (BCrypt.Net.BCrypt.Verify(request.NuevaContraseña, cliente.Contraseña))
                return false;

            // Actualizar contraseña
            cliente.Contraseña = BCrypt.Net.BCrypt.HashPassword(request.NuevaContraseña);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al cambiar contraseña del cliente ID: {clienteId}");
            throw;
        }
    }
}