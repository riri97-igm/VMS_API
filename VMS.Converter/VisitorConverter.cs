using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.EntityFramework.EntityModel;
using VMS.Model.DTOs.Visitor;

namespace VMS.Converter
{
    public static class VisitorConverter
    {
        public static VisitorDTO ToVisitorDTO(Visitor visitor)
        {
            return new VisitorDTO
            {
                Id = visitor.Id,
                FirstName = visitor.FirstName,
                LastName = visitor.LastName,
                Email = visitor.Email,
                Phone = visitor.Phone,
                Address = visitor.Address,
                Company = visitor.Company,
                IdentificationNumber = visitor.IdentificationNumber,
            };
        }

        public static Visitor ToVisitorEntity(VisitorDTO visitorDTO)
        {
            return new Visitor
            {
                Id = visitorDTO.Id,
                FirstName = visitorDTO.FirstName,
                LastName = visitorDTO.LastName,
                Email = visitorDTO.Email,
                Phone = visitorDTO.Phone,
                Address = visitorDTO.Address,
                Company = visitorDTO.Company,
                IdentificationNumber = visitorDTO.IdentificationNumber,
            };
        }
    }
}
