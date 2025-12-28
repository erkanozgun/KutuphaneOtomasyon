
using System.ComponentModel.DataAnnotations;

namespace Kutuphane.WebUI.Models.ViewModels
{
    public class LoanCreateViewModel
    {
        [Required(ErrorMessage = "Lütfen bir üye seçiniz.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir üye seçiniz.")]
        [Display(Name = "Üye")]
        public int MemberId { get; set; }

        public bool IsAutoSelection { get; set; } = true;

        [Display(Name = "Kitap")]
        public int? BookId { get; set; }

        [Display(Name = "Nüsha")]
        public int? CopyId { get; set; }

        [StringLength(300, ErrorMessage = "Notlar en fazla 300 karakter olabilir.")]
        [Display(Name = "Notlar")]
        public string? Notes { get; set; }
    }
}
