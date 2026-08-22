using System;

namespace Patholab.Shared.DTOs
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string PatientCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {(string.IsNullOrEmpty(MiddleName) ? "" : MiddleName + " ")}{LastName}".Trim();
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string? BloodGroup { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string? AlternateMobile { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactMobile { get; set; }
        public string? IdentityType { get; set; }
        public string? IdentityNumber { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
