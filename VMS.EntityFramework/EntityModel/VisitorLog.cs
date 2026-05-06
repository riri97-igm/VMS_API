namespace VMS.EntityFramework.EntityModel
{
    public class VisitorLog
    {
        public int Id { get; set; }
        public int VisitorId { get; set; }
        public Visitor Visitor { get; set; } = null!;
        public int StaffId { get; set; }
        public Staff Staff { get; set; } = null!;
        public int? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? Remarks { get; set; }
        public int ChangedBy { get; set; }
    }
}
