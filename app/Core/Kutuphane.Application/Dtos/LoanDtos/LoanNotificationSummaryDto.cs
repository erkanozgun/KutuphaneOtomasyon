using System;
using System.Collections.Generic;

namespace Kutuphane.Application.Dtos.LoanDtos;

/// <summary>
/// Gecikme bildirimi özeti için DTO
/// </summary>
public class LoanNotificationSummaryDto
{
    /// <summary>
    /// Gecikmiş toplam ödünç sayısı
    /// </summary>
    public int OverdueLoansCount { get; set; }

    /// <summary>
    /// Teslim tarihi yaklaşan ödünç sayısı (varsayılan: 3 gün içinde)
    /// </summary>
    public int NearingDueDateCount { get; set; }

    /// <summary>
    /// Bu ay yapılan toplam ödünç sayısı
    /// </summary>
    public int TotalLoansThisMonth { get; set; }

    /// <summary>
    /// Gecikmiş ödünçlerin listesi
    /// </summary>
    public List<ResultLoanDto> OverdueLoans { get; set; } = new();

    /// <summary>
    /// Teslim tarihi yaklaşan ödünçlerin listesi
    /// </summary>
    public List<ResultLoanDto> NearingDueLoans { get; set; } = new();

    /// <summary>
    /// Toplam gecikme günü (tüm gecikmiş ödünçler için)
    /// </summary>
    public int TotalOverdueDays { get; set; }

    /// <summary>
    /// En kritik gecikme (en fazla gün gecikmiş)
    /// </summary>
    public ResultLoanDto? MostCriticalOverdue { get; set; }

    /// <summary>
    /// Bildirimin oluşturulma tarihi
    /// </summary>
    public DateTime GeneratedAt { get; set; } = DateTime.Now;
}
