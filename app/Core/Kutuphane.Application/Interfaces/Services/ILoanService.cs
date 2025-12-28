using Kutuphane.Application.Dtos.AuthDtos;
using Kutuphane.Application.Dtos.LoanDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Interfaces.Services;

public interface ILoanService
{
    Task<ResultLoanDto> BorrowBookAsync(CreateLoanDto dto);
    Task<ResultLoanDto> ReturnBookAsync(int loanId, ReturnLoanDto dto);
    Task<ResultLoanDto?> GetLoanByIdAsync(int id);
    Task<ResultLoanDto?> GetActiveLoanAsync(int copyId);
    Task<IEnumerable<ResultLoanDto>> GetMemberActiveLoansAsync(int memberId);
    Task<IEnumerable<ResultLoanDto>> GetOverdueLoansAsync();
    Task<IEnumerable<ResultLoanDto>> GetLoanHistoryAsync(int memberId, int pageNumber, int pageSize);
    Task<bool> CanCopyBeBorrowedAsync(int copyId);
    Task<int> CalculateOverdueDaysAsync(int loanId);
    Task<IEnumerable<ResultLoanDto>> GetActiveLoansAsync();
    Task<IEnumerable<ResultLoanDto>> GetAllLoansAsync();

    // Yeni metodlar - Ödünç Düzenleme
    Task<ResultLoanDto> UpdateLoanAsync(UpdateLoanDto dto);
    Task<ResultLoanDto> ExtendLoanAsync(ExtendLoanDto dto);

    // Bildirim sistemi için
    Task<IEnumerable<ResultLoanDto>> GetLoansNearingDueDateAsync(int daysBeforeDue = 3);
    Task<LoanNotificationSummaryDto> GetNotificationSummaryAsync();
}
