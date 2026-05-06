using VMS.Model.DTOs.Appointment;

namespace VMS.DataAccess.Interface
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<AppointmentDetailsDTO>> GetAllAsync(); // changed from AppointmentDTO
        Task<AppointmentDetailsDTO?> GetByIdAsync(int id);
        Task<int> AddAsync(AppointmentDTO appointmentDto);
        Task UpdateAsync(AppointmentDTO appointmentDto);
        Task DeleteAsync(int id);
    }
}