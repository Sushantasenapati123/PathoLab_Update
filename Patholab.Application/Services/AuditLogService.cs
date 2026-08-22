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
    public class AuditLogService : IAuditLogService
    {
        private readonly IApplicationDbContext _context;

        public AuditLogService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(int? userId, string module, string action, string table, int recordId, string? oldVal, string? newVal, string? ip, string? ua, CancellationToken cancellationToken = default)
        {
            var log = new AuditLog
            {
                UserId = userId,
                ModuleName = module,
                Action = action,
                TableName = table,
                RecordId = recordId,
                OldValues = oldVal,
                NewValues = newVal,
                IPAddress = ip,
                UserAgent = ua,
                CreatedOn = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<AuditLogDto>> GetAuditLogsPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            var query = _context.AuditLogs
                .Include(x => x.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(x => x.ModuleName.ToLower().Contains(s) || 
                                         x.Action.ToLower().Contains(s) || 
                                         x.TableName.ToLower().Contains(s) ||
                                         (x.User != null && x.User.FullName.ToLower().Contains(s)));
            }

            var totalRecords = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var dtos = items.Select(x => new AuditLogDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Username = x.User?.Username ?? "System",
                FullName = x.User?.FullName ?? "System Process",
                ModuleName = x.ModuleName,
                Action = x.Action,
                TableName = x.TableName,
                RecordId = x.RecordId,
                OldValues = x.OldValues,
                NewValues = x.NewValues,
                IPAddress = x.IPAddress,
                UserAgent = x.UserAgent,
                CreatedOn = x.CreatedOn
            }).ToList();

            return new PagedResult<AuditLogDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }
    }
}
