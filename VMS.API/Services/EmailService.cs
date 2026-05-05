using System.Net;
using System.Net.Mail;

namespace VMS.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        private async Task SendAsync(string to, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(
                    _config["Email:SmtpHost"] ?? "smtp.gmail.com",
                    int.Parse(_config["Email:SmtpPort"] ?? "587"))
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(
                        _config["Email:Username"], _config["Email:Password"])
                };
                var msg = new MailMessage
                {
                    From = new MailAddress(_config["Email:FromAddress"]!, _config["Email:FromName"] ?? "VMS"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                msg.To.Add(to);
                await client.SendMailAsync(msg);
            }
            catch (Exception ex) { _logger.LogError(ex, "Email failed to {To}", to); }
        }

        public Task SendVisitorCheckInNotificationAsync(string hostEmail, string hostName,
            string visitorName, string visitorCompany, DateTime checkInTime, string purpose)
            => SendAsync(hostEmail, $"Visitor Arrived: {visitorName}",
                $"<p>Hi {hostName}, <strong>{visitorName}</strong> from <strong>{visitorCompany}</strong> checked in at {checkInTime:g} for: {purpose}</p>");

        public Task SendAppointmentApprovedAsync(string visitorEmail, string visitorName,
            string hostName, DateTime appointmentDate, string purpose)
            => SendAsync(visitorEmail, "Your Appointment is Approved",
                $"<p>Dear {visitorName}, your appointment with {hostName} on {appointmentDate:g} for '{purpose}' has been approved.</p>");

        public Task SendAppointmentRejectedAsync(string visitorEmail, string visitorName,
            string hostName, DateTime appointmentDate, string reason)
            => SendAsync(visitorEmail, "Appointment Update",
                $"<p>Dear {visitorName}, your appointment with {hostName} on {appointmentDate:g} could not be approved. Reason: {reason}</p>");
    }
}