using System.ComponentModel.DataAnnotations;

namespace Kutuphane.WebUI.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Kullanıcı adı 4-20 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]*$", ErrorMessage = "Kullanıcı adı harf ile başlamalı ve sadece harf, rakam ve alt çizgi içerebilir.")]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(80, ErrorMessage = "E-posta en fazla 80 karakter olabilir.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ad zorunludur.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Ad 2-30 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Ad sadece harflerden oluşmalıdır.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad zorunludur.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Soyad 2-30 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Soyad sadece harflerden oluşmalıdır.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon zorunludur.")]
        [RegularExpression(@"^(05\d{9}|5\d{9}|0\d{10}|\+90\d{10})$", ErrorMessage = "Geçerli bir Türkiye telefon numarası giriniz (05xx xxx xx xx).")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Adres en fazla 150 karakter olabilir.")]
        [Display(Name = "Adres")]
        public string? Address { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Şifre 8-30 karakter arasında olmalıdır.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#._\-])[A-Za-z\d@$!%*?&#._\-]{8,}$", ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter (@$!%*?&#._-) içermelidir.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifre Tekrar")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
