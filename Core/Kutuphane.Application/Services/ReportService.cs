using Kutuphane.Application.Dtos.ReportDtos;
using Kutuphane.Application.Interfaces.Repositories;
using Kutuphane.Application.Interfaces.Services;
using Kutuphane.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Services;

public class ReportService:IReportService
{
    private readonly IBookRepository _bookRepository;
    private readonly ICopyRepository _copyRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ILoanRepository _loanRepository;

    public ReportService(
        IBookRepository bookRepository,
        ICopyRepository copyRepository,
        IMemberRepository memberRepository,
        ILoanRepository loanRepository)
    {
        _bookRepository = bookRepository;
        _copyRepository = copyRepository;
        _memberRepository = memberRepository;
        _loanRepository = loanRepository;
    }

    public async Task<DashboardStatisticsDto> GetDashboardStatisticsAsync()
    {
        var totalBooks = await _bookRepository.CountAsync();
        var totalCopies = await _copyRepository.CountAsync();
        var availableCopies = await _copyRepository.CountAsync(c => c.Status == CopyStatus.Rafta);
        var totalMembers = await _memberRepository.CountAsync();
        var activeMembers = await _memberRepository.CountAsync(m => m.Status == MemberStatus.Aktif);
        var activeLoans = await _loanRepository.CountAsync(l => l.ReturnDate == null);

        var overdueLoans = (await _loanRepository.GetOverdueLoansAsync()).Count();

        return new DashboardStatisticsDto
        {
            TotalBooks = totalBooks,
            TotalCopies = totalCopies,
            AvailableCopies = availableCopies,
            TotalMembers = totalMembers,
            ActiveMembers = activeMembers,
            ActiveLoans = activeLoans,
            OverdueLoans = overdueLoans
        };
    }

    public async Task<IEnumerable<OverdueLoanReportDto>> GetOverdueLoansReportAsync()
    {
        var overdueLoans = await _loanRepository.GetOverdueLoansAsync();

        return overdueLoans.Select(loan => new OverdueLoanReportDto
        {
            LoanId = loan.Id,
            MemberName = loan.Member.FullName,
            MemberPhone = loan.Member.Phone,
            BookTitle = loan.Copy.Book.Title,
            CopyNumber = loan.Copy.CopyNumber,
            DueDate = loan.DueDate,
            OverdueDays = loan.OverdueDays
        });
    }

    public async Task<IEnumerable<PopularBookDto>> GetMostBorrowedBooksAsync(int count)
    {
        var books = await _bookRepository.GetMostBorrowedBooksAsync(count);

        return books.Select(book => new PopularBookDto
        {
            BookId = book.Id,
            ISBN = book.ISBN,
            Title = book.Title,
            Author = book.Author,
            BorrowCount = book.Copies.SelectMany(c => c.Loans).Count()
        });
    }

    public async Task<IEnumerable<ActiveMemberDto>> GetActiveMembersAsync()
    {
        var members = await _memberRepository.FindAsync(m => m.Status == MemberStatus.Aktif);

        var activeMemberDtos = new List<ActiveMemberDto>();

        foreach (var member in members)
        {
            var activeLoans = await _loanRepository.CountAsync(l => l.MemberId == member.Id && l.ReturnDate == null);
            var totalLoans = await _loanRepository.CountAsync(l => l.MemberId == member.Id);

            activeMemberDtos.Add(new ActiveMemberDto
            {
                MemberId = member.Id,
                MemberNumber = member.MemberNumber,
                FullName = member.FullName,
                ActiveLoanCount = activeLoans,
                TotalLoanCount = totalLoans
            });
        }

        return activeMemberDtos.OrderByDescending(m => m.TotalLoanCount);
    }

    public async Task<MonthlyStatisticsDto> GetMonthlyLoanStatisticsAsync(int year, int month)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var statistics = await _loanRepository.GetLoanStatisticsAsync(startDate, endDate);

        var newMembers = await _memberRepository.CountAsync(m =>
            m.RegistrationDate >= startDate && m.RegistrationDate <= endDate);

        var newBooks = await _bookRepository.CountAsync(b =>
            b.CreatedDate >= startDate && b.CreatedDate <= endDate);

        return new MonthlyStatisticsDto
        {
            Year = year,
            Month = month,
            TotalLoans = statistics.GetValueOrDefault("TotalLoans", 0),
            TotalReturns = statistics.GetValueOrDefault("TotalReturns", 0),
            NewMembers = newMembers,
            NewBooks = newBooks
        };
    }
}

