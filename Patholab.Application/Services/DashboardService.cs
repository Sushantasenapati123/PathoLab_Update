using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patholab.Application.Interfaces;
using Patholab.Domain.Enums;
using Patholab.Shared.DTOs;

namespace Patholab.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IApplicationDbContext _context;

        public DashboardService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
        {
            var today = DateTime.UtcNow.Date;

            var todayPatients = await _context.Patients
                .CountAsync(x => x.CreatedOn.Date == today && !x.DeletedFlag, cancellationToken);

            var todayOrders = await _context.Orders
                .CountAsync(x => x.OrderDate.Date == today && !x.DeletedFlag, cancellationToken);

            var todayTests = await _context.SampleTests
                .CountAsync(x => x.CreatedOn.Date == today && !x.DeletedFlag, cancellationToken);

            var pendingSamples = await _context.Samples
                .CountAsync(x => (x.SampleStatus == SampleStatus.Pending || x.SampleStatus == SampleStatus.Collected) && !x.DeletedFlag, cancellationToken);

            var pendingResults = await _context.SampleTests
                .CountAsync(x => x.Status == "Processing" && !x.DeletedFlag, cancellationToken);

            var pendingVerification = await _context.Orders
                .CountAsync(x => x.OrderStatus == OrderStatus.UnderVerification && !x.DeletedFlag, cancellationToken);

            var publishedReports = await _context.Reports
                .CountAsync(x => x.ReportStatus == ReportStatus.Published && !x.DeletedFlag, cancellationToken);

            var todayRevenue = await _context.Payments
                .Where(x => x.PaymentDate.Date == today && !x.DeletedFlag)
                .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0.00m;

            var outstandingAmount = await _context.Invoices
                .Where(x => x.InvoiceStatus != InvoiceStatus.Cancelled && !x.DeletedFlag)
                .SumAsync(x => (decimal?)x.DueAmount, cancellationToken) ?? 0.00m;

            return new DashboardSummaryDto
            {
                TodayPatients = todayPatients,
                TodayOrders = todayOrders,
                TodayTests = todayTests,
                PendingSamples = pendingSamples,
                PendingResults = pendingResults,
                PendingVerification = pendingVerification,
                PublishedReports = publishedReports,
                TodayRevenue = todayRevenue,
                OutstandingAmount = outstandingAmount
            };
        }

        public async Task<List<RevenueChartItem>> GetRevenueChartDataAsync(int days, CancellationToken cancellationToken = default)
        {
            var startDate = DateTime.UtcNow.Date.AddDays(-days);

            var payments = await _context.Payments
                .Where(p => p.PaymentDate.Date >= startDate && !p.DeletedFlag)
                .ToListAsync(cancellationToken);

            var grouped = payments
                .GroupBy(p => p.PaymentDate.Date)
                .Select(g => new RevenueChartItem
                {
                    Date = g.Key.ToString("dd MMM"),
                    Revenue = g.Sum(x => x.Amount)
                })
                .OrderBy(x => DateTime.Parse(x.Date))
                .ToList();

            // Fill in missing dates with zero revenue to ensure continuous chart flow
            var result = new List<RevenueChartItem>();
            for (int i = days; i >= 0; i--)
            {
                var targetDate = DateTime.UtcNow.Date.AddDays(-i);
                var dateStr = targetDate.ToString("dd MMM");
                var match = grouped.FirstOrDefault(x => x.Date == dateStr);

                result.Add(new RevenueChartItem
                {
                    Date = dateStr,
                    Revenue = match?.Revenue ?? 0.00m
                });
            }

            return result;
        }

        public async Task<List<TestStatsDto>> GetTestStatsAsync(int limit, CancellationToken cancellationToken = default)
        {
            var testsGrouped = await _context.SampleTests
                .Include(st => st.Test)
                .Where(st => !st.DeletedFlag)
                .GroupBy(st => st.Test.TestName)
                .Select(g => new TestStatsDto
                {
                    TestName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(limit)
                .ToListAsync(cancellationToken);

            return testsGrouped;
        }

        public async Task<List<DepartmentStatsDto>> GetDepartmentStatsAsync(CancellationToken cancellationToken = default)
        {
            var deptStats = await _context.SampleTests
                .Include(st => st.Test)
                .ThenInclude(t => t.Department)
                .Where(st => !st.DeletedFlag)
                .GroupBy(st => st.Test.Department.DepartmentName)
                .Select(g => new DepartmentStatsDto
                {
                    DepartmentName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync(cancellationToken);

            return deptStats;
        }
    }
}
