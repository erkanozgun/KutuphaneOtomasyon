using System.ComponentModel.DataAnnotations;

namespace Kutuphane.WebUI.Models.ViewModels
{
    public class BookEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kitap başlığı zorunludur.")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Kitap başlığı 1-150 karakter arasında olmalıdır.")]
        [Display(Name = "Kitap Adı")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Yazar adı 3-80 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s\.\-']+$", ErrorMessage = "Yazar adı sadece harflerden oluşmalıdır.")]
        [Display(Name = "Yazar")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN zorunludur.")]
        [RegularExpression(@"^(\d{10}|\d{13})$", ErrorMessage = "ISBN 10 veya 13 haneli olmalı ve sadece rakamlardan oluşmalıdır.")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori seçiniz.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Kategori 2-30 karakter arasında olmalıdır.")]
        [Display(Name = "Kategori")]
        public string Category { get; set; } = string.Empty;

        [Range(1, 5000, ErrorMessage = "Sayfa sayısı 1-5000 arasında olmalıdır.")]
        [Display(Name = "Sayfa Sayısı")]
        public int PageCount { get; set; }

        [Range(1450, 2025, ErrorMessage = "Yayın yılı 1450-2025 arasında geçerli bir yıl olmalıdır.")]
        [Display(Name = "Yayın Yılı")]
        public int PublicationYear { get; set; }

        [StringLength(40, MinimumLength = 2, ErrorMessage = "Yayınevi adı 2-40 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s\.\-'&]*$", ErrorMessage = "Yayınevi adı sayı içeremez.")]
        [Display(Name = "Yayınevi")]
        public string? Publisher { get; set; }

        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Yeni Kapak Resmi (Değiştirmek isterseniz seçin)")]
        public IFormFile? ImageFile { get; set; }

        public string? ExistingImageUrl { get; set; }
    }
}