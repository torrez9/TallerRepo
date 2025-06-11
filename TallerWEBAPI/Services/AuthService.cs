using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TallerWEBAPI.Models;

public class AuthService
{
    private readonly MotosTuning3Context _context;
    private readonly ILogger<AuthService> _logger;

    public AuthService(MotosTuning3Context context, ILogger<AuthService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Cliente?> ValidarCredenciales(string correo, string contraseña)
    {
        try
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Correo == correo);

            if (cliente == null)
            {
                _logger.LogWarning($"Intento de login con correo no existente: {correo}");
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(contraseña, cliente.Contraseña))
            {
                _logger.LogWarning($"Intento de login con contraseña incorrecta para: {correo}");
                return null;
            }

            return cliente;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al validar credenciales para: {correo}");
            throw;
        }
    }

    public async Task<RegistroResultado> RegistrarCliente(Cliente cliente)
    {
        try
        {
            if (await _context.Clientes.AnyAsync(c => c.Correo == cliente.Correo))
                return new RegistroResultado { Exito = false, MensajeError = "Ya existe un cliente con ese correo electrónico" };

            if (await _context.Clientes.AnyAsync(c => c.Usuario == cliente.Usuario))
                return new RegistroResultado { Exito = false, MensajeError = "El nombre de usuario ya está en uso" };

            cliente.Contraseña = BCrypt.Net.BCrypt.HashPassword(cliente.Contraseña);

            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Nuevo cliente registrado: {cliente.Correo}");
            return new RegistroResultado { Exito = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al registrar cliente: {cliente.Correo}");
            return new RegistroResultado { Exito = false, MensajeError = "Error interno al registrar el cliente" };
        }
    }

    public async Task<ResetPasswordResult> ResetPassword(string email, EmailService emailService)
    {
        try
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Correo == email);
            if (cliente == null)
            {
                _logger.LogWarning($"Intento de reset de contraseña para correo no existente: {email}");
                return new ResetPasswordResult { Exito = false, MensajeError = "No se encontró un cliente con ese correo electrónico" };
            }

            var newPassword = GenerateTemporaryPassword();
            cliente.Contraseña = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _context.SaveChangesAsync();
            await emailService.SendPasswordResetEmail(email, newPassword);

            _logger.LogInformation($"Contraseña reseteada para: {email}");
            return new ResetPasswordResult { Exito = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al resetear contraseña para: {email}");
            return new ResetPasswordResult { Exito = false, MensajeError = "Error interno al resetear la contraseña" };
        }
    }

    private string GenerateTemporaryPassword()
    {
        const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghjklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public string GenerarTokenJWT(Cliente cliente, IConfiguration configuration)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, cliente.IdCliente.ToString()),
                new Claim(ClaimTypes.Name, cliente.Nombre),
                new Claim(ClaimTypes.Email, cliente.Correo ?? string.Empty),
                new Claim(ClaimTypes.Role, "Cliente")
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar token JWT");
            throw;
        }
    }
}