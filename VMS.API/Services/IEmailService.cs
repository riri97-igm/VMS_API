namespace VMS.API.Services
{
    public interface IEmailService
    {
        Task SendVisitorCheckInNotificationAsync(string hostEmail, string hostName,
            string visitorName, string visitorCompany, DateTime checkInTime, string purpose);
        Task SendAppointmentApprovedAsync(string visitorEmail, string visitorName,
            string hostName, DateTime appointmentDate, string purpose);
        Task SendAppointmentRejectedAsync(string visitorEmail, string visitorName,
            string hostName, DateTime appointmentDate, string reason);
    }
}