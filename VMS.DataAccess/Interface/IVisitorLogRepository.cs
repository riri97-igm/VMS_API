using VMS.Model.DTOs.VisitorLog;

namespace VMS.DataAccess.Interface
{
    public interface IVisitorLogRepository
    {
        Task<IEnumerable<VisitorLogDetailsDTO>> GetAllAsync();
        Task<VisitorLogDetailsDTO?> GetByIdAsync(int id);
        Task<IEnumerable<VisitorLogDetailsDTO>> GetByVisitorIdAsync(int visitorId);
        Task<IEnumerable<VisitorLogDetailsDTO>> GetTodayLogsAsync();
        Task<int> CheckInAsync(VisitorLogDTO dto);
        Task<bool> CheckOutAsync(int id, string? remarks);
        Task DeleteAsync(int id);
    }
}
