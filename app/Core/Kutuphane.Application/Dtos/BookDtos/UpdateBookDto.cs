using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Dtos.BookDtos;

public class UpdateBookDto
{
    [Required(ErrorMessage = "Kitap başlığı zorunludur.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Kitap başlığı 1-150 karakter arasında olmalıdır.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yazar adı zorunludur.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Yazar adı 3-80 karakter arasında olmalıdır.")]
    [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s\.\-']+$", ErrorMessage = "Yazar adı sadece harflerden oluşmalıdır.")]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "ISBN zorunludur.")]
    [RegularExpression(@"^(\d{10}|\d{13})$", ErrorMessage = "ISBN 10 veya 13 haneli olmalı ve sadece rakamlardan oluşmalıdır.")]
    public string ISBN { get; set; } = string.Empty;

    [StringLength(40, MinimumLength = 2, ErrorMessage = "Yayınevi adı 2-40 karakter arasında olmalıdır.")]
    [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s\.\-'&]*$", ErrorMessage = "Yayınevi adı sayı içeremez.")]
    public string? Publisher { get; set; }

    [Range(1450, 2025, ErrorMessage = "Yayın yılı 1450-2025 arasında geçerli bir yıl olmalıdır.")]
    public int? PublicationYear { get; set; }

    [StringLength(30, MinimumLength = 2, ErrorMessage = "Kategori 2-30 karakter arasında olmalıdır.")]
    public string? Category { get; set; }

    [Range(1, 5000, ErrorMessage = "Sayfa sayısı 1-5000 arasında olmalıdır.")]
    public int? PageCount { get; set; }

    [Required(ErrorMessage = "Dil zorunludur.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Dil 2-20 karakter arasında olmalıdır.")]
    public string Language { get; set; } = "Türkçe";

    [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
    public string? Description { get; set; }

    [StringLength(300, ErrorMessage = "Resim URL'si en fazla 300 karakter olabilir.")]
    public string? ImageUrl { get; set; }
}
