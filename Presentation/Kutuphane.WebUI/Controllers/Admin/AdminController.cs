using Kutuphane.Application.Interfaces.Services;
using Kutuphane.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kutuphane.WebUI.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IReportService _reportService;

        public AdminController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IActionResult> Index()
        {
          
            var stats = await _reportService.GetDashboardStatisticsAsync();


            var overdueReport = await _reportService.GetOverdueLoansReportAsync();

    
            var popularBooks = await _reportService.GetMostBorrowedBooksAsync(5);

            var model = new AdminDashboardViewModel
            {
                Stats = stats,
                OverdueLoans = overdueReport.ToList(), 
                PopularBooks = popularBooks.ToList()   
            };

            return View(model);
        }
    }
}
