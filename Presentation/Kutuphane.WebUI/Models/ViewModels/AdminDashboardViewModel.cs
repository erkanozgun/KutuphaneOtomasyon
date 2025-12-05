using Kutuphane.Application.Dtos.ReportDtos;

namespace Kutuphane.WebUI.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public DashboardStatisticsDto Stats { get; set; }
        public List<OverdueLoanReportDto> OverdueLoans { get; set; }
        public List<PopularBookDto> PopularBooks { get; set; }

        // İleride grafik eklemek istersen:
        // public MonthlyStatisticsDto MonthlyStats { get; set; }
    }
}
