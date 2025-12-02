using Kutuphane.Application.Dtos.AuthDtos;
using Kutuphane.Application.Dtos.LoanDtos;
using Kutuphane.Application.Exceptions;
using Kutuphane.Application.Interfaces.Repositories;
using Kutuphane.Application.Interfaces.Services;
using Kutuphane.Domain.Entities;
using Kutuphane.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly ICopyRepository _copyRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IUserRepository _userRepository;
    private const int LoanDurationDays = 14;

    public LoanService(
        ILoanRepository loanRepository,
        ICopyRepository copyRepository,
        IMemberRepository memberRepository,
        IUserRepository userRepository)
    {
        _loanRepository = loanRepository;
        _copyRepository = copyRepository;
        _memberRepository = memberRepository;
        _userRepository = userRepository;
    }

    public async Task<ResultLoanDto> BorrowBookAsync(CreateLoanDto dto)
    {
        // 1. Kopya kontrolü
        var copy = await _copyRepository.GetCopyWithBookAsync(dto.CopyId);
        if (copy == null)
        {
            throw new NotFoundException("Copy", dto.CopyId);
        }

        // 2. Kopya müsait mi?
        if (!await CanCopyBeBorrowedAsync(dto.CopyId))
        {
            throw new CopyNotAvailableException(dto.CopyId);
        }

        // 3. Üye kontrolü
        var member = await _memberRepository.GetByIdAsync(dto.MemberId);
        if (member == null)
        {
            throw new NotFoundException("Member", dto.MemberId);
        }

        // 4. Üye ödünç alabilir mi?
        if (member.Status != MemberStatus.Aktif)
        {
            throw new MemberNotEligibleException($"Member status is '{member.Status}'. Only active members can borrow books.");
        }

        // 5. Gecikmiş iadesi var mı?
        var activeLoans = await _loanRepository.GetMemberActiveLoansAsync(dto.MemberId);
        var hasOverdueLoans = activeLoans.Any(l => l.IsOverdue);
        if (hasOverdueLoans)
        {
            throw new MemberNotEligibleException("Member has overdue loans. Cannot borrow new books.");
        }

        // 6. Kullanıcı kontrolü
        var user = await _userRepository.GetByIdAsync(dto.LoanedByUserId);
        if (user == null)
        {
            throw new NotFoundException("User", dto.LoanedByUserId);
        }

        // 7. Ödünç kaydı oluştur
        var loan = new Loan
        {
            CopyId = dto.CopyId,
            MemberId = dto.MemberId,
            LoanDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(LoanDurationDays),
            Status = LoanStatus.Aktif,
            Notes = dto.Notes,
            LoanedByUserId = dto.LoanedByUserId
        };

        await _loanRepository.AddAsync(loan);

        // 8. Kopya durumunu güncelle
        copy.Status = CopyStatus.Oduncte;
        await _copyRepository.UpdateAsync(copy);

        // 9. DTO döndür
        return new ResultLoanDto
        {
            Id = loan.Id,
            CopyId = loan.CopyId,
            MemberId = loan.MemberId,
            MemberName = member.FullName,
            BookTitle = copy.Book.Title,
            CopyNumber = copy.CopyNumber,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnDate = loan.ReturnDate,
            Status = loan.Status.ToString(),
            Notes = loan.Notes,
            BookId=copy.BookId,
            LoanedByUserId = loan.LoanedByUserId,
            LoanedByUserName = user.FullName,
            IsOverdue = loan.IsOverdue,
            OverdueDays = loan.OverdueDays
        };
    }

    public async Task<ResultLoanDto> ReturnBookAsync(int loanId, ReturnLoanDto dto)
    {
        // 1. Ödünç kaydı bul
        var loan = await _loanRepository.GetLoanWithDetailsAsync(loanId);
        if (loan == null)
        {
            throw new NotFoundException("Loan", loanId);
        }

        // 2. Zaten iade edilmiş mi?
        if (loan.ReturnDate != null)
        {
            throw new BusinessException("This book has already been returned.");
        }

        // 3. İade işlemini yap
        loan.ReturnDate = DateTime.Now;
        loan.Status = LoanStatus.IadeEdildi;

        if (!string.IsNullOrWhiteSpace(dto.Notes))
        {
            loan.Notes = loan.Notes + " | Return Note: " + dto.Notes;
        }

        await _loanRepository.UpdateAsync(loan);

        // 4. Kopya durumunu güncelle
        var copy = await _copyRepository.GetByIdAsync(loan.CopyId);
        if (copy != null)
        {
            copy.Status = CopyStatus.Rafta;
            await _copyRepository.UpdateAsync(copy);
        }

        // 5. DTO döndür
        return new ResultLoanDto
        {
            Id = loan.Id,
            CopyId = loan.CopyId,
            MemberId = loan.MemberId,
            MemberName = loan.Member.FullName,
            BookTitle = loan.Copy.Book.Title,
            CopyNumber = loan.Copy.CopyNumber,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnDate = loan.ReturnDate,
            Status = loan.Status.ToString(),
            Notes = loan.Notes,
            LoanedByUserId = loan.LoanedByUserId,
            LoanedByUserName = loan.LoanedByUser.FullName,
            IsOverdue = loan.IsOverdue,
            OverdueDays = loan.OverdueDays
        };
    }

    public async Task<ResultLoanDto?> GetLoanByIdAsync(int id)
    {
        var loan = await _loanRepository.GetLoanWithDetailsAsync(id);
        return loan != null ? MapToDto(loan) : null;
    }

    public async Task<ResultLoanDto?> GetActiveLoanAsync(int copyId)
    {
        var loan = await _loanRepository.GetActiveLoanAsync(copyId);
        return loan != null ? MapToDto(loan) : null;
    }

    public async Task<IEnumerable<ResultLoanDto>> GetMemberActiveLoansAsync(int memberId)
    {
        var loans = await _loanRepository.GetMemberActiveLoansAsync(memberId);
        return loans.Select(MapToDto);
    }

    public async Task<IEnumerable<ResultLoanDto>> GetOverdueLoansAsync()
    {
        var loans = await _loanRepository.GetOverdueLoansAsync();
        return loans.Select(MapToDto);
    }

    public async Task<IEnumerable<ResultLoanDto>> GetLoanHistoryAsync(int memberId, int pageNumber, int pageSize)
    {
        var loans = await _loanRepository.GetLoanHistoryAsync(memberId, pageNumber, pageSize);
        return loans.Select(MapToDto);
    }

    public async Task<bool> CanCopyBeBorrowedAsync(int copyId)
    {
        var copy = await _copyRepository.GetByIdAsync(copyId);
        if (copy == null)
            return false;

        // Kopya rafta mı?
        if (copy.Status != CopyStatus.Rafta)
            return false;

        // Aktif ödünç var mı?
        var activeLoan = await _loanRepository.GetActiveLoanAsync(copyId);
        return activeLoan == null;
    }

    public async Task<int> CalculateOverdueDaysAsync(int loanId)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId);
        if (loan == null)
        {
            throw new NotFoundException("Loan", loanId);
        }

        return loan.OverdueDays;
    }

    private ResultLoanDto MapToDto(Loan loan)
    {
        return new ResultLoanDto
        {
            Id = loan.Id,
            CopyId = loan.CopyId,
            MemberId = loan.MemberId,
            MemberName = loan.Member.FullName,
            BookTitle = loan.Copy.Book.Title,
            CopyNumber = loan.Copy.CopyNumber,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnDate = loan.ReturnDate,
            Status = loan.Status.ToString(),
            Notes = loan.Notes,
            LoanedByUserId = loan.LoanedByUserId,
            LoanedByUserName = loan.LoanedByUser.FullName,
            IsOverdue = loan.IsOverdue,
            OverdueDays = loan.OverdueDays
        };
    }
}
