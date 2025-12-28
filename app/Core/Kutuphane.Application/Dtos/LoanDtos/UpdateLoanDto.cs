using System;
using System.ComponentModel.DataAnnotations;

namespace Kutuphane.Application.Dtos.LoanDtos;

public class UpdateLoanDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Teslim tarihi zorunludur.")]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }

    [MaxLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir.")]
    public string? Notes { get; set; }
}

public class ExtendLoanDto
{
    [Required]
    public int Id { get; set; }

    [Range(1, 30, ErrorMessage = "Uzatma süresi 1-30 gün arasında olmalıdır.")]
    public int ExtensionDays { get; set; } = 7;

    public string? ExtensionReason { get; set; }
}
