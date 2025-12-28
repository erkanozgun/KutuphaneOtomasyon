using Kutuphane.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Dtos.AuthDtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Ad zorunludur.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Ad 2-30 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Ad sadece harflerden oluşmalıdır.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad zorunludur.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Soyad 2-30 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Soyad sadece harflerden oluşmalıdır.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Kullanıcı adı 4-20 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]*$", ErrorMessage = "Kullanıcı adı harf ile başlamalı ve sadece harf, rakam ve alt çizgi içerebilir.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [StringLength(80, ErrorMessage = "E-posta en fazla 80 karakter olabilir.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Şifre 8-30 karakter arasında olmalıdır.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#._\-])[A-Za-z\d@$!%*?&#._\-]{8,}$", ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.")]
        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; }
    }
}
