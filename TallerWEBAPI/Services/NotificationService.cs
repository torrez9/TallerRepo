using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TallerWEBAPI.Models;

namespace TallerWEBAPI.Services
{
    public interface INotificationService
    {
        Task NotificarCitasDelDia();
        Task EnviarNotificacionCita(Cita cita, Cliente cliente);
        Task EnviarNotificacionCita(int citaId);
    }

    public class NotificationService : INotificationService
    {
        private readonly CitaService _citaService;
        private readonly ClienteService _clienteService;

        // ADVERTENCIA: No es recomendable hardcodear credenciales en el código
        private const string EmailFrom = "darwincarballo44@gmail.com";
        private const string EmailPassword = "jfzslqsbbfjtiicp";
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;

        public NotificationService(
            CitaService citaService,
            ClienteService clienteService)
        {
            _citaService = citaService;
            _clienteService = clienteService;
        }

        public async Task NotificarCitasDelDia()
        {
            var citasHoy = await _citaService.ObtenerCitasDeHoyAsync();
            var tasks = new List<Task>();

            foreach (var cita in citasHoy)
            {
                var cliente = await _clienteService.ObtenerClientePorIdAsync(cita.IdCliente);
                if (cliente != null && !string.IsNullOrEmpty(cliente.Correo))
                {
                    tasks.Add(EnviarNotificacionCita(cita, cliente));
                }
            }

            await Task.WhenAll(tasks);
        }

        public async Task EnviarNotificacionCita(int citaId)
        {
            var cita = await _citaService.ObtenerCitaPorIdAsync(citaId);
            if (cita == null)
                throw new ArgumentException("Cita no encontrada");

            var cliente = await _clienteService.ObtenerClientePorIdAsync(cita.IdCliente);
            if (cliente == null)
                throw new ArgumentException("Cliente no encontrado");

            await EnviarNotificacionCita(cita, cliente);
        }

        public async Task EnviarNotificacionCita(Cita cita, Cliente cliente)
        {
            try
            {
                var horaCita = cita.Hora.HasValue ? cita.Hora.Value.ToString("hh\\:mm") : "horario no especificado";

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(EmailFrom));
                email.To.Add(MailboxAddress.Parse(cliente.Correo));
                email.Subject = "🔧 Recordatorio de cita en Taller Motos Tuning";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $@"
                <html>
                <head>
                    <style>
                        @media only screen and (max-width: 600px) {{
                            .container {{
                                padding: 10px !important;
                                width: 100% !important;
                            }}
                            .button {{
                                font-size: 16px !important;
                                padding: 10px !important;
                            }}
                        }}
                    </style>
                </head>
                <body style=""margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f9f9f9;"">
                    <div class=""container"" style=""max-width: 600px; margin: auto; border: 1px solid #e0e0e0; border-radius: 8px; padding: 20px; background-color: #ffffff;"">
                        <div style=""text-align: center;"">
                            <h2 style=""color: #2e7d32;"">📅 Recordatorio de cita</h2>
                        </div>
                        <p style=""font-size: 16px; color: #333;"">Hola {cliente.Nombre},</p>
                        <p style=""font-size: 16px; color: #333;"">Este es un recordatorio de que tienes una cita programada para hoy en <strong>Taller Motos Tuning</strong>.</p>
        
                        <div style=""background-color: #f5f5f5; padding: 15px; border-radius: 5px; margin: 20px 0;"">
                            <p style=""font-size: 16px; margin: 5px 0;""><strong>Fecha:</strong> {cita.FechaCita.ToString("dd/MM/yyyy")}</p>
                            <p style=""font-size: 16px; margin: 5px 0;""><strong>Hora:</strong> {horaCita}</p>
                            <p style=""font-size: 16px; margin: 5px 0;""><strong>Descripción:</strong> {cita.Descripcion}</p>
                        </div>

                        <p style=""font-size: 16px; color: #333;"">Por favor, asegúrate de llegar a tiempo para tu cita.</p>
                        <p style=""font-size: 16px; color: #333;"">Si necesitas reprogramar o cancelar, por favor contáctanos con anticipación.</p>
        
                        <hr style=""border: none; border-top: 1px solid #ccc; margin: 30px 0;"">
                        <p style=""font-size: 14px; color: #666;"">Atentamente,<br>Equipo de Taller Motos Tuning</p>
                    </div>
                </body>
                </html>";

                email.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.StartTls);

                // Autenticación con las credenciales proporcionadas
                await smtp.AuthenticateAsync(EmailFrom, EmailPassword);

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enviando notificación a {cliente.Correo}: {ex.Message}");
                throw;
            }
        }
    }
}