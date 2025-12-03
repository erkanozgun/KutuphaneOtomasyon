using Kutuphane.Application.Dtos.AuthDtos;
using Kutuphane.Application.Dtos.MemberDtos;
using Kutuphane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kutuphane.WebUI.Controllers.Member
{
    public class MembersController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMemberService _memberService;
        private readonly ILoanService _loanService;
        public MembersController(IAuthService authService, IMemberService memberService, ILoanService loanService)
        {
            _authService = authService;
            _memberService = memberService;
            _loanService = loanService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _authService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            // MemberId varsa member bilgilerini çek
            if (user.MemberId.HasValue)
            {
                var member = await _memberService.GetMemberByIdAsync(user.MemberId.Value);
                ViewBag.Member = member;
            }

            return View(user);
        }
        [HttpGet]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> MyLoans()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _authService.GetUserByIdAsync(userId);

            if (user == null || !user.MemberId.HasValue)
            {
                TempData["Error"] = "Üye bilgileriniz bulunamadı.";
                return RedirectToAction("Profile");
            }

            // Üyenin aktif ödünçleri
            var activeLoans = await _loanService.GetMemberActiveLoansAsync(user.MemberId.Value);

            // Üyenin geçmiş ödünçleri
            var loanHistory = await _loanService.GetLoanHistoryAsync(user.MemberId.Value, 1, 10);

            ViewBag.ActiveLoans = activeLoans;
            ViewBag.LoanHistory = loanHistory;



            return View();
        }
        //Profill Güncelleme
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _authService.GetUserByIdAsync(userId); // User bilgilerini çekiyoruz

            if (user == null || !user.MemberId.HasValue)
                return RedirectToAction("Index", "Home");

            var member = await _memberService.GetMemberByIdAsync(user.MemberId.Value);

            var dto = new MemberProfileDto
            {
                Id = member.Id,

                // BURASI EKLENDİ: Mevcut kullanıcı adını ekrana basıyoruz
                Username = user.Username,

                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                Phone = member.Phone,
                Address = member.Address,
                DateOfBirth = member.DateOfBirth
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(MemberProfileDto model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
            
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var user = await _authService.GetUserByIdAsync(userId);

                if (user.MemberId != model.Id)
                {
                    TempData["Error"] = "Hatalı işlem.";
                    return RedirectToAction("Profile");
                }

                // Servisi çağır
                await _memberService.UpdateMemberProfileAsync(model);

                TempData["Success"] = "Profil bilgileriniz güncellendi.";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(model);
            }
        }
        // --- ŞİFRE DEĞİŞTİRME ---

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // AuthService zaten bu işi yapıyor
                await _authService.ChangePasswordAsync(userId, model);

                TempData["Success"] = "Şifreniz başarıyla değiştirildi.";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                // "Mevcut şifre yanlış" gibi hatalar buradan döner
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        
    }
}

