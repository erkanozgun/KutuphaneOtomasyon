using Kutuphane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kutuphane.WebUI.ViewComponents
{
    /// <summary>
    /// Üyelerin gecikmiş ve yaklaşan teslimlerini bildirim olarak gösteren ViewComponent
    /// </summary>
    public class MemberNotificationViewComponent : ViewComponent
    {
        private readonly ILoanService _loanService;
        private readonly IAuthService _authService;

        public MemberNotificationViewComponent(ILoanService loanService, IAuthService authService)
        {
            _loanService = loanService;
            _authService = authService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Kullanıcı giriş yapmamışsa gösterme
            if (!User.Identity?.IsAuthenticated ?? true)
                return Content(string.Empty);

            // Admin/Librarian kullanıcılar için gösterme (onlar zaten admin panelden görür)
            if (HttpContext.User.IsInRole("Admin") || HttpContext.User.IsInRole("Librarian"))
                return Content(string.Empty);

            try
            {
                // Kullanıcının UserId'sini al
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                    return Content(string.Empty);

                // User'dan MemberId'yi al
                var user = await _authService.GetUserByIdAsync(userId);
                if (user == null || !user.MemberId.HasValue)
                    return Content(string.Empty);

                int memberId = user.MemberId.Value;

                // Üyenin aktif ödünçlerini al
                var activeLoans = await _loanService.GetMemberActiveLoansAsync(memberId);
                var loanList = activeLoans.ToList();

                if (!loanList.Any())
                    return Content(string.Empty);

                // Gecikmiş ödünçler
                var overdueLoans = loanList.Where(l => l.IsOverdue).ToList();

                // Yaklaşan teslimler (3 gün içinde)
                var nearingLoans = loanList.Where(l =>
                    !l.IsOverdue &&
                    l.DueDate.HasValue &&
                    (l.DueDate.Value.Date - DateTime.Now.Date).TotalDays <= 3 &&
                    (l.DueDate.Value.Date - DateTime.Now.Date).TotalDays >= 0
                ).ToList();

                var model = new MemberNotificationViewModel
                {
                    OverdueLoans = overdueLoans,
                    NearingDueLoans = nearingLoans,
                    TotalOverdueDays = overdueLoans.Sum(l => l.OverdueDays),
                    HasNotifications = overdueLoans.Any() || nearingLoans.Any()
                };

                return View(model);
            }
            catch
            {
                // Hata durumunda sessizce geç
                return Content(string.Empty);
            }
        }
    }

    public class MemberNotificationViewModel
    {
        public List<Kutuphane.Application.Dtos.LoanDtos.ResultLoanDto> OverdueLoans { get; set; } = new();
        public List<Kutuphane.Application.Dtos.LoanDtos.ResultLoanDto> NearingDueLoans { get; set; } = new();
        public int TotalOverdueDays { get; set; }
        public bool HasNotifications { get; set; }
    }
}
