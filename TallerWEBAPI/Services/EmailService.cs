using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendPasswordResetEmail(string toEmail, string newPassword)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("darwincarballo44@gmail.com"));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = "🔧 Taller Motos Tuning - Recuperación de Contraseña";

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $@"
        <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 8px; padding: 20px; background-color: #f9f9f9;"">
            <div style=""text-align: center;"">
                <h2 style=""color: #2e7d32;"">🔐 Recuperación de Contraseña</h2>
            </div>
            <p style=""font-size: 16px; color: #333;"">Hola,</p>
            <p style=""font-size: 16px; color: #333;"">Hemos recibido una solicitud para restablecer tu contraseña de acceso al sistema de <strong>Taller Motos Tuning</strong>.</p>
            <p style=""font-size: 16px; color: #333;"">Tu nueva contraseña temporal es:</p>
            <div style=""text-align: center; margin: 20px 0;"">
                <span style=""display: inline-block; background-color: #2e7d32; color: white; padding: 10px 20px; border-radius: 5px; font-size: 18px; font-weight: bold;"">{newPassword}</span>
            </div>
            <p style=""font-size: 16px; color: #333;"">Por tu seguridad, te recomendamos cambiar esta contraseña una vez que inicies sesión.</p>
            <hr style=""border: none; border-top: 1px solid #ccc; margin: 30px 0;"">
            <p style=""font-size: 14px; color: #666;"">Si no solicitaste este cambio, por favor ignora este correo o contacta con nuestro equipo de soporte.</p>
            <p style=""font-size: 14px; color: #666;"">Atentamente,<br>Equipo de Taller Motos Tuning</p>
        </div>";

        email.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync("darwincarballo44@gmail.com", "jfzslqsbbfjtiicp");
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}