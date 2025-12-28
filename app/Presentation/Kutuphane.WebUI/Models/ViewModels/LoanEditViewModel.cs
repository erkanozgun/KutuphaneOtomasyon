using System;
using System.ComponentModel.DataAnnotations;

namespace Kutuphane.WebUI.Models.ViewModels;

public class LoanEditViewModel
{
    public int Id { get; set; }

    // Sadece gösterim için - değiştirilemez
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int CopyId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string CopyNumber { get; set; } = string.Empty;
    public string? Author { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = string.Empty;

    // Düzenlenebilir alanlar
    [Required(ErrorMessage = "Teslim tarihi zorunludur.")]
    [DataType(DataType.Date)]
    [Display(Name = "Son Teslim Tarihi")]
    public DateTime DueDate { get; set; }

    [MaxLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir.")]
    [Display(Name = "Notlar")]
    public string? Notes { get; set; }

    // Hesaplanan alanlar
    public bool IsOverdue => ReturnDate == null && DateTime.Now.Date > DueDate.Date;
    public int OverdueDays => IsOverdue ? (DateTime.Now.Date - DueDate.Date).Days : 0;
    public int RemainingDays => !IsOverdue && ReturnDate == null ? (DueDate.Date - DateTime.Now.Date).Days : 0;
}

public class LoanExtendViewModel
{
    public int Id { get; set; }

    // Gösterim için
    public string MemberName { get; set; } = string.Empty;
    public string BookTitle { get; set; } = string.Empty;
    public DateTime CurrentDueDate { get; set; }
    public DateTime NewDueDate { get; set; }

    [Range(1, 30, ErrorMessage = "Uzatma süresi 1-30 gün arasında olmalıdır.")]
    [Display(Name = "Uzatma Süresi (Gün)")]
    public int ExtensionDays { get; set; } = 7;

    [Display(Name = "Uzatma Nedeni")]
    public string? ExtensionReason { get; set; }
}
