namespace VMS.Model.DTOs.Staff
{
    public class StaffDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int DepartmetId { get; set; }
        public string DepartmentName { get; set; } = string.Empty; // added for display
        public int RoleId { get; set; }
    }
}