﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Model.DTOs.Visitor
{
    public class VisitorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Company { get; set; } = string.Empty;
        public string? IdentificationNumber { get; set; } = string.Empty;
        public int ChangeBy { get; set; }
    }
}
