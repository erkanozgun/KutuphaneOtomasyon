using Kutuphane.Application.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Dtos.MemberDtos;

public class CreateMemberDto
{
    [Required(ErrorMessage = "Ad zorunludur.")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "Ad 2-30 karakter arasında olmalıdır.")]
    [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Ad sadece harflerden oluşmalıdır.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad zorunludur.")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "Soyad 2-30 karakter arasında olmalıdır.")]
    [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Soyad sadece harflerden oluşmalıdır.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-Posta alanı zorunludur.")]
    [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta formatı giriniz.")]
    [StringLength(80, ErrorMessage = "E-posta en fazla 80 karakter olabilir.")]
    [ValidEmailDomain(ErrorMessage = "Girdiğiniz e-posta adresinin alan adı (domain) geçersiz.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Telefon numarası zorunludur.")]
    [RegularExpression(@"^(05\d{9}|5\d{9}|0\d{10}|\+90\d{10})$", ErrorMessage = "Geçerli bir Türkiye telefon numarası giriniz (05xx xxx xx xx veya 5xxxxxxxxx).")]
    public string Phone { get; set; } = string.Empty;

    [StringLength(150, ErrorMessage = "Adres en fazla 150 karakter olabilir.")]
    public string? Address { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(300, ErrorMessage = "Notlar en fazla 300 karakter olabilir.")]
    public string? Notes { get; set; }
}
