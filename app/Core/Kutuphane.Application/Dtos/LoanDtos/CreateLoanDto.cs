using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Dtos.LoanDtos;

public class CreateLoanDto
{
    [Required(ErrorMessage = "Nüsha seçimi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir nüsha seçiniz.")]
    public int CopyId { get; set; }

    [Required(ErrorMessage = "Kitap seçimi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kitap seçiniz.")]
    public int BookId { get; set; }

    [Required(ErrorMessage = "Üye seçimi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir üye seçiniz.")]
    public int MemberId { get; set; }

    [Required(ErrorMessage = "İşlemi yapan kullanıcı bilgisi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kullanıcı gereklidir.")]
    public int LoanedByUserId { get; set; }

    [StringLength(300, ErrorMessage = "Notlar en fazla 300 karakter olabilir.")]
    public string? Notes { get; set; }
}
