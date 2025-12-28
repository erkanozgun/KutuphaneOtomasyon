using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Kutuphane.WebUI.Models.ViewModels
{
    public class SendMessageViewModel
    {
        [Display(Name = "Alıcı Üye")]
        [Required(ErrorMessage = "Lütfen bir üye seçiniz.")]
        public string ReceiverId { get; set; } = string.Empty;

        [Display(Name = "Konu")]
        [Required(ErrorMessage = "Konu başlığı zorunludur.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Konu başlığı 3-100 karakter arasında olmalıdır.")]
        public string Subject { get; set; } = string.Empty;

        [Display(Name = "Mesaj İçeriği")]
        [Required(ErrorMessage = "Mesaj içeriği boş olamaz.")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Mesaj içeriği 10-2000 karakter arasında olmalıdır.")]
        public string Content { get; set; } = string.Empty;


        public IEnumerable<SelectListItem>? MemberList { get; set; }
    }
}
