using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TallerWEBAPI.Models;

public class AuthService
{
    private readonly MotosTuningContext _context;

    public AuthService(MotosTuningContext context)
    {
        _context = context;
    }

    public Usuario? ValidarCredenciales(string correo, string contraseña)
    {
        var usuario = _context.Usuarios
            .Include(u => u.IdRolNavigation)
            .FirstOrDefault(u => u.Correo == correo);

        if (usuario == null) return null;

        // Verificar contraseña hasheada
        return BCrypt.Net.BCrypt.Verify(contraseña, usuario.Contraseña) ? usuario : null;
    }


    //restaurar usuario
    public bool RegistrarUsuario(Usuario usuario)
    {
        if (_context.Usuarios.Any(u => u.Correo == usuario.Correo))
            return false;

        // Hashear la contraseña antes de guardar
        usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        return true;
    }


    //Restaurar Contra
    public async Task<bool> ResetPassword(string email, EmailService emailService)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Correo == email);
        if (usuario == null) return false;

        // Generar nueva contraseña temporal
        var newPassword = GenerateTemporaryPassword();

        // Hashear la nueva contraseña
        usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(newPassword);

        _context.SaveChanges();

        // Enviar correo con la nueva contraseña
        await emailService.SendPasswordResetEmail(email, newPassword);

        return true;
    }

    //contra tenporal
    private string GenerateTemporaryPassword()
    {
        const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghjklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }


    //genera token jwt
    public string GenerarTokenJWT(Usuario usuario, IConfiguration configuration)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.IdRolNavigation?.NombreRol ?? "Cliente")
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}