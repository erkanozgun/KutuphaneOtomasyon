using Kutuphane.Application.Dtos.MemberDtos;
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

public class MemberService:IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILoanRepository _loanRepository;

    public MemberService(IMemberRepository memberRepository, ILoanRepository loanRepository)
    {
        _memberRepository = memberRepository;
        _loanRepository = loanRepository;
    }

    public async Task<IEnumerable<ResultMemberDto>> GetAllMembersAsync()
    {
        var members = await _memberRepository.GetAllAsync();
        return members.Select(MapToDto);
    }

    public async Task<ResultMemberDto?> GetMemberByIdAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        return member != null ? MapToDto(member) : null;
    }

    public async Task<MemberWithLoansDto?> GetMemberWithLoansAsync(int id)
    {
        var member = await _memberRepository.GetMemberWithLoansAsync(id);
        if (member == null)
            return null;

        var activeLoans = member.Loans.Where(l => l.ReturnDate == null).ToList();
        var loanHistory = member.Loans.Where(l => l.ReturnDate != null).OrderByDescending(l => l.LoanDate).ToList();

        return new MemberWithLoansDto
        {
            Id = member.Id,
            MemberNumber = member.MemberNumber,
            FirstName = member.FirstName,
            LastName = member.LastName,
            FullName = member.FullName,
            Email = member.Email,
            Phone = member.Phone,
            Address = member.Address,
            DateOfBirth = member.DateOfBirth,
            RegistrationDate = member.RegistrationDate,
            Status = member.Status.ToString(),
            Notes = member.Notes,
            ActiveLoans = activeLoans.Select(l => new Dtos.LoanDtos.ResultLoanDto
            {
                Id = l.Id,
                CopyId = l.CopyId,
                MemberId = l.MemberId,
                MemberName = member.FullName,
                BookTitle = l.Copy.Book.Title,
                CopyNumber = l.Copy.CopyNumber,
                LoanDate = l.LoanDate,
                DueDate = l.DueDate,
                ReturnDate = l.ReturnDate,
                Status = l.Status.ToString(),
                Notes = l.Notes,
                LoanedByUserId = l.LoanedByUserId,
                LoanedByUserName = l.LoanedByUser.FullName,
                IsOverdue = l.IsOverdue,
                OverdueDays = l.OverdueDays
            }),
            LoanHistory = loanHistory.Select(l => new Dtos.LoanDtos.ResultLoanDto
            {
                Id = l.Id,
                CopyId = l.CopyId,
                MemberId = l.MemberId,
                MemberName = member.FullName,
                BookTitle = l.Copy.Book.Title,
                CopyNumber = l.Copy.CopyNumber,
                LoanDate = l.LoanDate,
                DueDate = l.DueDate,
                ReturnDate = l.ReturnDate,
                Status = l.Status.ToString(),
                Notes = l.Notes,
                LoanedByUserId = l.LoanedByUserId,
                LoanedByUserName = l.LoanedByUser.FullName,
                IsOverdue = l.IsOverdue,
                OverdueDays = l.OverdueDays
            })
        };
    }

    public async Task<IEnumerable<ResultMemberDto>> SearchMembersAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllMembersAsync();

        var members = await _memberRepository.SearchMembersAsync(searchTerm);
        return members.Select(MapToDto);
    }

    public async Task<bool> CanMemberBorrowAsync(int memberId)
    {
        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null)
            return false;

        // Üye aktif mi?
        if (member.Status != MemberStatus.Aktif)
            return false;

        // Gecikmiş iadesi var mı?
        var activeLoans = await _loanRepository.GetMemberActiveLoansAsync(memberId);
        var hasOverdueLoans = activeLoans.Any(l => l.IsOverdue);

        return !hasOverdueLoans;
    }

    public async Task<MemberStatisticsDto> GetMemberStatisticsAsync(int memberId)
    {
        var member = await _memberRepository.GetByIdAsync(memberId);
        if (member == null)
        {
            throw new NotFoundException("Member", memberId);
        }

        var allLoans = await _loanRepository.FindAsync(l => l.MemberId == memberId);
        var activeLoans = allLoans.Where(l => l.ReturnDate == null).ToList();
        var overdueLoans = activeLoans.Where(l => l.IsOverdue).ToList();
        var completedLoans = allLoans.Where(l => l.ReturnDate != null).ToList();

        return new MemberStatisticsDto
        {
            MemberId = memberId,
            MemberName = member.FullName,
            TotalLoans = allLoans.Count(),
            ActiveLoans = activeLoans.Count,
            OverdueLoans = overdueLoans.Count,
            CompletedLoans = completedLoans.Count
        };
    }

    public async Task<ResultMemberDto> CreateMemberAsync(CreateMemberDto dto)
    {
        // Telefon kontrolü (opsiyonel - iş kuralına göre)
        var existingMember = await _memberRepository.FirstOrDefaultAsync(m => m.Phone == dto.Phone);
        if (existingMember != null)
        {
            throw new DuplicateException("Member", "Phone", dto.Phone);
        }

        // Email kontrolü
        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            var existingEmail = await _memberRepository.FirstOrDefaultAsync(m => m.Email == dto.Email);
            if (existingEmail != null)
            {
                throw new DuplicateException("Member", "Email", dto.Email);
            }
        }

        // Üye numarası oluştur
        var memberNumber = await GenerateMemberNumberAsync();

        var member = new Member
        {
            MemberNumber = memberNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            DateOfBirth = dto.DateOfBirth,
            RegistrationDate = DateTime.Now,
            Status = MemberStatus.Aktif,
            Notes = dto.Notes
        };

        await _memberRepository.AddAsync(member);

        return MapToDto(member);
    }

    public async Task<ResultMemberDto> UpdateMemberAsync(int id, UpdateMemberDto dto)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member == null)
        {
            throw new NotFoundException("Member", id);
        }

        // Email kontrolü
        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != member.Email)
        {
            var existingEmail = await _memberRepository.FirstOrDefaultAsync(m => m.Email == dto.Email && m.Id != id);
            if (existingEmail != null)
            {
                throw new DuplicateException("Member", "Email", dto.Email);
            }
        }

        member.FirstName = dto.FirstName;
        member.LastName = dto.LastName;
        member.Email = dto.Email;
        member.Phone = dto.Phone;
        member.Address = dto.Address;
        member.DateOfBirth = dto.DateOfBirth;
        member.Notes = dto.Notes;

        if (Enum.TryParse<MemberStatus>(dto.Status, out var newStatus))
        {
            member.Status = newStatus;
        }

        await _memberRepository.UpdateAsync(member);

        return MapToDto(member);
    }

    public async Task DeleteMemberAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member == null)
        {
            throw new NotFoundException("Member", id);
        }

        // Aktif ödünç var mı kontrol et
        var hasActiveLoans = await _memberRepository.HasActiveLoansAsync(id);
        if (hasActiveLoans)
        {
            throw new BusinessException("Cannot delete member with active loans. Return all books first.");
        }

        await _memberRepository.DeleteAsync(id);
    }

    private async Task<string> GenerateMemberNumberAsync()
    {
        var year = DateTime.Now.Year;
        var count = await _memberRepository.CountAsync();
        return $"UYE-{year}-{(count + 1):D4}"; // Örnek: UYE-2025-0001
    }

    private ResultMemberDto MapToDto(Member member)
    {
        return new ResultMemberDto
        {
            Id = member.Id,
            MemberNumber = member.MemberNumber,
            FirstName = member.FirstName,
            LastName = member.LastName,
            FullName = member.FullName,
            Email = member.Email,
            Phone = member.Phone,
            Address = member.Address,
            DateOfBirth = member.DateOfBirth,
            RegistrationDate = member.RegistrationDate,
            Status = member.Status.ToString(),
            Notes = member.Notes
        };
    }
}
