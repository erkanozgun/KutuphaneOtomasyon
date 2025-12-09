using Kutuphane.Application.Interfaces.Services; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Kutuphane.WebUI.Controllers.Admin
{
    [Authorize(Roles = "Admin,Librarian")]
    public class AdminContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IUserService _userService;

        public AdminContactController(IContactService contactService, IUserService userService)
        {
            _contactService = contactService;
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var values = await _contactService.GetAllMessagesAsync();
            return View(values);
        }

  
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var message = await _contactService.GetMessageByIdAsync(id);

            if (message == null)
            {
                return RedirectToAction("Index");
            }

            if (!message.IsRead)
            {
                await _contactService.MarkAsReadAsync(id);
            }

            return View(message);
        }


        public async Task<IActionResult> Delete(int id)
        {
            await _contactService.DeleteMessageAsync(id);
            return RedirectToAction("Index");
        }

    
        [HttpPost]
        public async Task<IActionResult> Reply(int id, string replyMessage)
        {
            await _contactService.ReplyToMessageAsync(id, replyMessage);

        
            TempData["Success"] = "Cevap başarıyla gönderildi.";
            return RedirectToAction("Index");
        }
        // Controllers/AdminContactController.cs içine ekle:

        //[HttpGet]
        //public async Task<IActionResult> SendMessage(string receiverId = null)
        //{
        //    // 1. Tüm üyeleri (Admin olmayanları veya herkesi) getirip Dropdown için hazırlayalım
        //    // Not: _userManager ve _context'in tanımlı olduğunu varsayıyorum.
        //    var members = _userManager.Users.ToList();

        //    var model = new SendMessageViewModel
        //    {
        //        // Eğer bir butona basıp geldiyse o kişiyi otomatik seç
        //        ReceiverId = receiverId,

        //        // Dropdown listesini doldur (Value=Id, Text=Ad Soyad/Email)
        //        MemberList = members.Select(u => new SelectListItem
        //        {
        //            Value = u.Id,
        //            Text = $"{u.UserName} ({u.Email})"
        //        }).ToList()
        //    };

        //    // Eğer gecikme uyarısı için gelindiyse konuyu otomatik doldurabiliriz
        //    if (!string.IsNullOrEmpty(receiverId))
        //    {
        //        model.Subject = "Kütüphane Kitap İade Hatırlatması";
        //        model.Content = "Sayın üyemiz, üzerinizde bulunan kitabın iade süresi geçmiştir. Lütfen en kısa sürede iade ediniz.";
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> SendMessage(SendMessageViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Mesajı veritabanına kaydetme işlemi
        //        var message = new Message // Senin Message Entity sınıfın
        //        {
        //            SenderId = _userService.GetUserId(User), // Gönderen Admin
        //            ReceiverId = model.ReceiverId,           // Seçilen Üye
        //            Subject = model.Subject,
        //            Content = model.Content,
        //            SentDate = DateTime.Now,
        //            IsRead = false
        //        };

        //        _context.Messages.Add(message);
        //        await _context.SaveChangesAsync();

        //        TempData["Success"] = "Mesaj başarıyla gönderildi.";
        //        return RedirectToAction("Index"); // Mesajlar listesine dön
        //    }

        //    // Hata varsa listeyi tekrar doldurup sayfayı geri döndür
        //    model.MemberList = _userManager.Users.Select(u => new SelectListItem
        //    {
        //        Value = u.Id,
        //        Text = $"{u.UserName} ({u.Email})"
        //    }).ToList();

        //    return View(model);
        //}
    }
}