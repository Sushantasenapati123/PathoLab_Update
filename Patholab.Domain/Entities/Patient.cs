using System;
using System.Collections.Generic;

namespace Patholab.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public string PatientCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
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

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public virtual ICollection<HomeCollection> HomeCollections { get; set; } = new List<HomeCollection>();
    }
}
