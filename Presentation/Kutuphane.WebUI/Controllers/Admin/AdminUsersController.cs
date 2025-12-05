using Kutuphane.Application.Dtos.AuthDtos;
using Kutuphane.Application.Interfaces.Services;
using Kutuphane.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kutuphane.WebUI.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserService _userService;

        public AdminUsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDto model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var dto = new CreateUserDto
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password,
                    Role = model.Role
                };

                await _userService.CreateUserAsync(dto);

                TempData["Success"] = "Personel hesabı ve üye kaydı başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Hata: " + ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Kendi kendini silme koruması
                var currentUser = User.Identity.Name;
                var targetUser = await _userService.GetUserByIdAsync(id);

                if (targetUser.Username == currentUser)
                {
                    TempData["Error"] = "Güvenlik gereği kendi hesabınızı silemezsiniz.";
                    return RedirectToAction("Index");
                }

                await _userService.DeleteUserAsync(id);
                TempData["Success"] = "Kullanıcı başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Silme hatası: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}