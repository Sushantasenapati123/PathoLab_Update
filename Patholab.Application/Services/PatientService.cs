using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patholab.Application.Interfaces;
using Patholab.Domain.Entities;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IApplicationDbContext _context;

        public PatientService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PatientDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var p = await _context.Patients
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);
            if (p == null) throw new Exception("Patient not found.");
            return MapToDto(p);
        }

        public async Task<PagedResult<PatientDto>> GetAllPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            var query = _context.Patients.Where(x => !x.DeletedFlag);

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(x => x.PatientCode.ToLower().Contains(s) || 
                                         x.FirstName.ToLower().Contains(s) || 
                                         x.LastName.ToLower().Contains(s) || 
                                         x.Mobile.Contains(s));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => MapToDto(x))
                .ToListAsync(cancellationToken);

            return new PagedResult<PatientDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task<PatientDto> CreateAsync(PatientDto dto, CancellationToken cancellationToken = default)
        {
            // Check for duplicates (by Mobile and Name)
            var duplicate = await _context.Patients.AnyAsync(x => 
                x.Mobile == dto.Mobile && x.FirstName == dto.FirstName && x.LastName == dto.LastName && !x.DeletedFlag, 
                cancellationToken);

            if (duplicate)
            {
                throw new Exception("A patient with the same name and mobile number already exists.");
            }

            // Generate Patient Code
            var count = await _context.Patients.CountAsync(cancellationToken) + 1;
            var patientCode = $"PAT-{count:D6}";

            var p = new Patient
            {
                PatientCode = patientCode,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                Age = dto.Age,
                BloodGroup = dto.BloodGroup,
                Mobile = dto.Mobile,
                AlternateMobile = dto.AlternateMobile,
                Email = dto.Email,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                Pincode = dto.Pincode,
                EmergencyContactName = dto.EmergencyContactName,
                EmergencyContactMobile = dto.EmergencyContactMobile,
                IdentityType = dto.IdentityType,
                IdentityNumber = dto.IdentityNumber,
                Remarks = dto.Remarks,
                IsActive = dto.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System"
            };

            _context.Patients.Add(p);
            await _context.SaveChangesAsync(cancellationToken);
            return MapToDto(p);
        }

        public async Task<PatientDto> UpdateAsync(int id, PatientDto dto, CancellationToken cancellationToken = default)
        {
            var p = await _context.Patients
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);
            if (p == null) throw new Exception("Patient not found.");

            p.FirstName = dto.FirstName;
            p.MiddleName = dto.MiddleName;
            p.LastName = dto.LastName;
            p.Gender = dto.Gender;
            p.DateOfBirth = dto.DateOfBirth;
            p.Age = dto.Age;
            p.BloodGroup = dto.BloodGroup;
            p.Mobile = dto.Mobile;
            p.AlternateMobile = dto.AlternateMobile;
            p.Email = dto.Email;
            p.Address = dto.Address;
            p.City = dto.City;
            p.State = dto.State;
            p.Pincode = dto.Pincode;
            p.EmergencyContactName = dto.EmergencyContactName;
            p.EmergencyContactMobile = dto.EmergencyContactMobile;
            p.IdentityType = dto.IdentityType;
            p.IdentityNumber = dto.IdentityNumber;
            p.Remarks = dto.Remarks;
            p.IsActive = dto.IsActive;
            p.UpdatedOn = DateTime.UtcNow;
            p.UpdatedBy = "System";

            await _context.SaveChangesAsync(cancellationToken);
            return MapToDto(p);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var p = await _context.Patients
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);
            if (p == null) throw new Exception("Patient not found.");

            p.DeletedFlag = true;
            p.UpdatedOn = DateTime.UtcNow;
            p.UpdatedBy = "System";

            await _context.SaveChangesAsync(cancellationToken);
        }

        private static PatientDto MapToDto(Patient p)
        {
            return new PatientDto
            {
                Id = p.Id,
                PatientCode = p.PatientCode,
                FirstName = p.FirstName,
                MiddleName = p.MiddleName,
                LastName = p.LastName,
                Gender = p.Gender,
                DateOfBirth = p.DateOfBirth,
                Age = p.Age,
                BloodGroup = p.BloodGroup,
                Mobile = p.Mobile,
                AlternateMobile = p.AlternateMobile,
                Email = p.Email,
                Address = p.Address,
                City = p.City,
                State = p.State,
                Pincode = p.Pincode,
                EmergencyContactName = p.EmergencyContactName,
                EmergencyContactMobile = p.EmergencyContactMobile,
                IdentityType = p.IdentityType,
                IdentityNumber = p.IdentityNumber,
                Remarks = p.Remarks,
                IsActive = p.IsActive
            };
        }
    }
}
