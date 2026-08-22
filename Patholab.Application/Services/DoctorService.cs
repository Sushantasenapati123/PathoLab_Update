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
    public class DoctorService : IDoctorService
    {
        private readonly IApplicationDbContext _context;

        public DoctorService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var d = await _context.Doctors
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);
            if (d == null) throw new Exception("Doctor not found.");
            return MapToDto(d);
        }

        public async Task<PagedResult<DoctorDto>> GetAllPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            var query = _context.Doctors.Where(x => !x.DeletedFlag);

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(x => x.DoctorCode.ToLower().Contains(s) || 
                                         x.DoctorName.ToLower().Contains(s) || 
                                         x.Mobile.Contains(s) || 
                                         x.Specialization!.ToLower().Contains(s));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => MapToDto(x))
                .ToListAsync(cancellationToken);

            return new PagedResult<DoctorDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        public async Task<DoctorDto> CreateAsync(DoctorDto dto, CancellationToken cancellationToken = default)
        {
            // Generate Doctor Code
            var count = await _context.Doctors.CountAsync(cancellationToken) + 1;
            var doctorCode = $"DOC-{count:D6}";

            var d = new Doctor
            {
                DoctorCode = doctorCode,
                DoctorName = dto.DoctorName,
                Qualification = dto.Qualification,
                Specialization = dto.Specialization,
                HospitalClinicName = dto.HospitalClinicName,
                Mobile = dto.Mobile,
                Email = dto.Email,
                Address = dto.Address,
                CommissionType = dto.CommissionType,
                CommissionValue = dto.CommissionValue,
                IsActive = dto.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System"
            };

            _context.Doctors.Add(d);
            await _context.SaveChangesAsync(cancellationToken);
            return MapToDto(d);
        }

        public async Task<DoctorDto> UpdateAsync(int id, DoctorDto dto, CancellationToken cancellationToken = default)
        {
            var d = await _context.Doctors
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);
            if (d == null) throw new Exception("Doctor not found.");

            d.DoctorName = dto.DoctorName;
            d.Qualification = dto.Qualification;
            d.Specialization = dto.Specialization;
            d.HospitalClinicName = dto.HospitalClinicName;
            d.Mobile = dto.Mobile;
            d.Email = dto.Email;
            d.Address = dto.Address;
            d.CommissionType = dto.CommissionType;
            d.CommissionValue = dto.CommissionValue;
            d.IsActive = dto.IsActive;
            d.UpdatedOn = DateTime.UtcNow;
            d.UpdatedBy = "System";

            await _context.SaveChangesAsync(cancellationToken);
            return MapToDto(d);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var d = await _context.Doctors
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);
            if (d == null) throw new Exception("Doctor not found.");

            d.DeletedFlag = true;
            d.UpdatedOn = DateTime.UtcNow;
            d.UpdatedBy = "System";

            await _context.SaveChangesAsync(cancellationToken);
        }

        private static DoctorDto MapToDto(Doctor d)
        {
            return new DoctorDto
            {
                Id = d.Id,
                DoctorCode = d.DoctorCode,
                DoctorName = d.DoctorName,
                Qualification = d.Qualification,
                Specialization = d.Specialization,
                HospitalClinicName = d.HospitalClinicName,
                Mobile = d.Mobile,
                Email = d.Email,
                Address = d.Address,
                CommissionType = d.CommissionType,
                CommissionValue = d.CommissionValue,
                IsActive = d.IsActive
            };
        }
    }
}
