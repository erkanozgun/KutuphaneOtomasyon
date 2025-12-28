using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Dtos.ContactDtos
{
    public class CreateContactMessageDto
    {
        public int? SelectedMemberId { get; set; }

        [Required(ErrorMessage = "E-Posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "İsim alanı gereklidir.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "İsim 2-100 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "İsim sadece harflerden oluşmalıdır.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konu başlığı gereklidir.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Konu başlığı 3-100 karakter arasında olmalıdır.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mesaj içeriği gereklidir.")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Mesaj içeriği 10-2000 karakter arasında olmalıdır.")]
        public string Message { get; set; } = string.Empty;
    }
}

