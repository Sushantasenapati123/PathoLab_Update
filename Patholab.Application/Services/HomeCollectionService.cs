using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patholab.Application.Interfaces;
using Patholab.Domain.Entities;
using Patholab.Domain.Enums;
using Patholab.Shared.DTOs;
using Patholab.Shared.Models;

namespace Patholab.Application.Services
{
    public class HomeCollectionService : IHomeCollectionService
    {
        private readonly IApplicationDbContext _context;

        public HomeCollectionService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HomeCollectionDto> RequestCollectionAsync(RequestHomeCollectionRequest request, CancellationToken cancellationToken = default)
        {
            var count = await _context.HomeCollections.CountAsync(cancellationToken) + 1;
            var requestNumber = $"HC-2026-{count:D6}";

            var hc = new HomeCollection
            {
                RequestNumber = requestNumber,
                PatientId = request.PatientId,
                Address = request.Address,
                City = request.City,
                Pincode = request.Pincode,
                RequestedDate = request.RequestedDate,
                RequestedTime = request.RequestedTime,
                Status = HomeCollectionStatus.Requested,
                Remarks = request.Remarks,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System"
            };

            _context.HomeCollections.Add(hc);
            await _context.SaveChangesAsync(cancellationToken);

            return await GetByIdAsync(hc.Id, cancellationToken);
        }

        public async Task AssignAgentAsync(int hcId, int agentId, CancellationToken cancellationToken = default)
        {
            var hc = await _context.HomeCollections.FindAsync(new object[] { hcId }, cancellationToken);
            if (hc == null) throw new Exception("Home collection request not found.");

            var agentExists = await _context.Users
                .AnyAsync(u => u.Id == agentId && u.Role.RoleName == "Collection Agent" && u.IsActive && !u.DeletedFlag, cancellationToken);
            if (!agentExists) throw new Exception("Invalid collection agent selected.");

            hc.AssignedAgentId = agentId;
            hc.Status = HomeCollectionStatus.Assigned;
            hc.UpdatedOn = DateTime.UtcNow;
            hc.UpdatedBy = "System";

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateStatusAsync(int hcId, HomeCollectionStatus status, string? remarks, CancellationToken cancellationToken = default)
        {
            var hc = await _context.HomeCollections.FindAsync(new object[] { hcId }, cancellationToken);
            if (hc == null) throw new Exception("Home collection request not found.");

            hc.Status = status;
            if (!string.IsNullOrEmpty(remarks))
            {
                hc.Remarks = (hc.Remarks + $"\n[{status}: {remarks}]").Trim();
            }

            if (status == HomeCollectionStatus.SampleCollected)
            {
                hc.CollectionDateTime = DateTime.UtcNow;
            }

            hc.UpdatedOn = DateTime.UtcNow;
            hc.UpdatedBy = "System";

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<HomeCollectionDto>> GetPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            var query = _context.HomeCollections
                .Include(x => x.Patient)
                .Include(x => x.Order)
                .Include(x => x.AssignedAgent)
                .Where(x => !x.DeletedFlag);

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(x => x.RequestNumber.ToLower().Contains(s) || 
                                         x.Patient.FirstName.ToLower().Contains(s) || 
                                         x.Patient.LastName.ToLower().Contains(s) ||
                                         x.City.ToLower().Contains(s) ||
                                         x.Status.ToString().ToLower().Contains(s));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var dtos = items.Select(hc => new HomeCollectionDto
            {
                Id = hc.Id,
                RequestNumber = hc.RequestNumber,
                PatientId = hc.PatientId,
                PatientName = $"{hc.Patient.FirstName} {hc.Patient.LastName}",
                PatientMobile = hc.Patient.Mobile,
                OrderId = hc.OrderId,
                OrderNumber = hc.Order?.OrderNumber,
                Address = hc.Address,
                City = hc.City,
                Pincode = hc.Pincode,
                RequestedDate = hc.RequestedDate,
                RequestedTime = hc.RequestedTime,
                AssignedAgentId = hc.AssignedAgentId,
                AssignedAgentName = hc.AssignedAgent?.FullName,
                Status = hc.Status,
                CollectionDateTime = hc.CollectionDateTime,
                Remarks = hc.Remarks
            }).ToList();

            return new PagedResult<HomeCollectionDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }

        private async Task<HomeCollectionDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var hc = await _context.HomeCollections
                .Include(x => x.Patient)
                .Include(x => x.Order)
                .Include(x => x.AssignedAgent)
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedFlag, cancellationToken);

            if (hc == null) throw new Exception("Home collection request not found.");

            return new HomeCollectionDto
            {
                Id = hc.Id,
                RequestNumber = hc.RequestNumber,
                PatientId = hc.PatientId,
                PatientName = $"{hc.Patient.FirstName} {hc.Patient.LastName}",
                PatientMobile = hc.Patient.Mobile,
                OrderId = hc.OrderId,
                OrderNumber = hc.Order?.OrderNumber,
                Address = hc.Address,
                City = hc.City,
                Pincode = hc.Pincode,
                RequestedDate = hc.RequestedDate,
                RequestedTime = hc.RequestedTime,
                AssignedAgentId = hc.AssignedAgentId,
                AssignedAgentName = hc.AssignedAgent?.FullName,
                Status = hc.Status,
                CollectionDateTime = hc.CollectionDateTime,
                Remarks = hc.Remarks
            };
        }
    }
}
