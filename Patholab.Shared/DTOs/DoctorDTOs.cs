using Patholab.Domain.Enums;

namespace Patholab.Shared.DTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string DoctorCode { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public string? HospitalClinicName { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public CommissionType CommissionType { get; set; } = CommissionType.None;
        public decimal CommissionValue { get; set; } = 0.00m;
        public bool IsActive { get; set; } = true;
    }
}
