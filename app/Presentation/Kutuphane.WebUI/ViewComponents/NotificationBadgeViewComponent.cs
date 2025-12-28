using Kutuphane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kutuphane.WebUI.ViewComponents
{
    /// <summary>
    /// Navbar'da kullanıcının gecikmiş ödünçlerini gösteren küçük badge
    /// </summary>
    public class NotificationBadgeViewComponent : ViewComponent
    {
        private readonly ILoanService _loanService;
        private readonly IAuthService _authService;

        public NotificationBadgeViewComponent(ILoanService loanService, IAuthService authService)
        {
            _loanService = loanService;
            _authService = authService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Kullanıcı giriş yapmamışsa gösterme
            if (!User.Identity?.IsAuthenticated ?? true)
                return Content(string.Empty);

            // Admin/Librarian için gösterme
            if (HttpContext.User.IsInRole("Admin") || HttpContext.User.IsInRole("Librarian"))
                return Content(string.Empty);

            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                    return Content(string.Empty);

                var user = await _authService.GetUserByIdAsync(userId);
                if (user == null || !user.MemberId.HasValue)
                    return Content(string.Empty);

                var activeLoans = await _loanService.GetMemberActiveLoansAsync(user.MemberId.Value);
                var overdueCount = activeLoans.Count(l => l.IsOverdue);
                var nearingCount = activeLoans.Count(l =>
                    !l.IsOverdue &&
                    l.DueDate.HasValue &&
                    (l.DueDate.Value.Date - DateTime.Now.Date).TotalDays <= 3
                );

                var model = new NotificationBadgeViewModel
                {
                    OverdueCount = overdueCount,
                    NearingDueCount = nearingCount,
                    TotalAlerts = overdueCount + nearingCount
                };

                return View(model);
            }
            catch
            {
                return Content(string.Empty);
            }
        }
    }

    public class NotificationBadgeViewModel
    {
        public int OverdueCount { get; set; }
        public int NearingDueCount { get; set; }
        public int TotalAlerts { get; set; }
    }
}
