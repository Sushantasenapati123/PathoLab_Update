using System.Collections.Generic;

namespace Patholab.Shared.DTOs
{
    public class DashboardSummaryDto
    {
        public int TodayPatients { get; set; }
        public int TodayOrders { get; set; }
        public int TodayTests { get; set; }
        public int PendingSamples { get; set; }
        public int PendingResults { get; set; }
        public int PendingVerification { get; set; }
        public int PublishedReports { get; set; }
        public decimal TodayRevenue { get; set; }
        public decimal OutstandingAmount { get; set; }
    }

    public class RevenueChartItem
    {
        public string Date { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }

    public class TestStatsDto
    {
        public string TestName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class DepartmentStatsDto
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
