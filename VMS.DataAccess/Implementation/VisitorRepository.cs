using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VMS.Converter;
using VMS.DataAccess.Interface;
using VMS.EntityFramework;
using VMS.Model.DTOs.Visitor;

namespace VMS.DataAccess.Implementation
{
    public class VisitorRepository : IVisitorRepository
    {
        private readonly DbVMSDataContext _context;
        private readonly IVisitorRepository _visitorRepository;

        public VisitorRepository(DbVMSDataContext context, IVisitorRepository visitorRepository)
        {
            _context = context;
            _visitorRepository = visitorRepository;
        }
        public async Task<IEnumerable<VisitorDTO>> GetAllAsync()
        {
            var visitors = await _context.Visitors.ToListAsync();
            return visitors.Select(VisitorConverter.ToVisitorDTO);
        }
        public async Task<int> AddAsync (VisitorDTO visitorDTO)
        {
            var visitor = VisitorConverter.ToVisitorEntity(visitorDTO);
            _context.Visitors.Add(visitor);
            await _context.SaveChangesAsync();

            return visitor.Id;
        }
        public async Task<VisitorDTO?> GetByIdAsync(int id)
        {
            var visitor = await _context.Visitors.FindAsync(id);
            return visitor == null ? null : VisitorConverter.ToVisitorDTO(visitor);
        }

        public async Task UpdateAsync (VisitorDTO visitorDTO)
        {
            var existingVisitor = await _context.Visitors.FindAsync(visitorDTO.Id);
            if (existingVisitor == null)
                return;
            var oldValues = JsonSerializer.Serialize(existingVisitor);
            existingVisitor.FirstName = visitorDTO.FirstName;  
            existingVisitor.LastName = visitorDTO.LastName;
            existingVisitor.Email = visitorDTO.Email;
            existingVisitor.Phone = visitorDTO.Phone;
            existingVisitor.Address = visitorDTO.Address;
            existingVisitor.Company = visitorDTO.Company;
            existingVisitor.IdentificationNumber = visitorDTO.IdentificationNumber;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor == null)
                return ;

            _context.Visitors.Remove(visitor);
            await _context.SaveChangesAsync();
        }
    }
}
