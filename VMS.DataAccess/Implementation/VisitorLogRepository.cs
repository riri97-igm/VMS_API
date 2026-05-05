using Microsoft.EntityFrameworkCore;
using VMS.Converter;
using VMS.DataAccess.Interface;
using VMS.EntityFramework;
using VMS.Model.DTOs.VisitorLog;

namespace VMS.DataAccess.Implementation
{
    public class VisitorLogRepository : IVisitorLogRepository
    {
        private readonly DbVMSDataContext _context;

        public VisitorLogRepository(DbVMSDataContext context)
        {
            _context = context;
        }

        private IQueryable<EntityFramework.EntityModel.VisitorLog> BaseQuery() =>
            _context.VisitorLogs
                .Include(l => l.Visitor)
                .Include(l => l.Staff)
                .Include(l => l.Appointment);

        public async Task<IEnumerable<VisitorLogDetailsDTO>> GetAllAsync()
        {
            var logs = await BaseQuery().OrderByDescending(l => l.CheckInTime).ToListAsync();
            return logs.Select(VisitorLogConverter.ToVisitorLogDetailsDTO);
        }

        public async Task<VisitorLogDetailsDTO?> GetByIdAsync(int id)
        {
            var log = await BaseQuery().FirstOrDefaultAsync(l => l.Id == id);
            return log == null ? null : VisitorLogConverter.ToVisitorLogDetailsDTO(log);
        }

        public async Task<IEnumerable<VisitorLogDetailsDTO>> GetByVisitorIdAsync(int visitorId)
        {
            var logs = await BaseQuery()
                .Where(l => l.VisitorId == visitorId)
                .OrderByDescending(l => l.CheckInTime)
                .ToListAsync();
            return logs.Select(VisitorLogConverter.ToVisitorLogDetailsDTO);
        }

        public async Task<IEnumerable<VisitorLogDetailsDTO>> GetTodayLogsAsync()
        {
            var today = DateTime.UtcNow.Date;
            var logs = await BaseQuery()
                .Where(l => l.CheckInTime.Date == today)
                .OrderByDescending(l => l.CheckInTime)
                .ToListAsync();
            return logs.Select(VisitorLogConverter.ToVisitorLogDetailsDTO);
        }

        public async Task<int> CheckInAsync(VisitorLogDTO dto)
        {
            var log = VisitorLogConverter.ToVisitorLogEntity(dto);
            log.CheckInTime = DateTime.UtcNow;
            _context.VisitorLogs.Add(log);
            await _context.SaveChangesAsync();
            return log.Id;
        }

        public async Task<bool> CheckOutAsync(int id, string? remarks)
        {
            var log = await _context.VisitorLogs.FindAsync(id);
            if (log == null) return false;
            log.CheckOutTime = DateTime.UtcNow;
            if (remarks != null) log.Remarks = remarks;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var log = await _context.VisitorLogs.FindAsync(id);
            if (log == null) return;
            _context.VisitorLogs.Remove(log);
            await _context.SaveChangesAsync();
        }
    }
}
