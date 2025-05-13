using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Model.DTOs.Appointment;

namespace VMS.DataAccess.Interface
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<AppointmentDTO>> GetAllAsync();
        Task<AppointmentDetailsDTO?> GetByIdAsync(int id);
        Task<int> AddAsync (AppointmentDTO appointmentDto);
        Task UpdateAsync (AppointmentDTO appointmentDto);
        Task DeleteAsync (int id);
    }
}
