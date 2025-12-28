using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Dtos.CopyDtos;

public class CreateCopyDto
{
    [Required(ErrorMessage = "Kitap seçimi zorunludur.")]
    [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kitap seçiniz.")]
    public int BookId { get; set; }

    [Required(ErrorMessage = "Nüsha numarası zorunludur.")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "Nüsha numarası 1-20 karakter arasında olmalıdır.")]
    public string CopyNumber { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Raf konumu en fazla 50 karakter olabilir.")]
    public string? ShelfLocation { get; set; }

    [Required(ErrorMessage = "Edinme tarihi zorunludur.")]
    [DataType(DataType.Date)]
    public DateTime AcquisitionDate { get; set; }

    [Range(0, 100000, ErrorMessage = "Fiyat 0-100000 arasında olmalıdır.")]
    public decimal? Price { get; set; }

    [StringLength(50, ErrorMessage = "Durum en fazla 50 karakter olabilir.")]
    public string? Condition { get; set; }
}
