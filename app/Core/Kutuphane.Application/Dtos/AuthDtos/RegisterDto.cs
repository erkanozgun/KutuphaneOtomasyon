using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Dtos.AuthDtos;

public class RegisterDto
{
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3-50 karakter arasında olmalıdır.")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Şifre en az 8, en fazla 100 karakter olmalıdır.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ad zorunludur.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad 2-50 karakter arasında olmalıdır.")]
    [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Ad sadece harflerden oluşmalıdır.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad zorunludur.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyad 2-50 karakter arasında olmalıdır.")]
    [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Soyad sadece harflerden oluşmalıdır.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Rol zorunludur.")]
    [StringLength(20, ErrorMessage = "Rol en fazla 20 karakter olabilir.")]
    public string Role { get; set; } = string.Empty;

    public int? MemberId { get; set; }
}
