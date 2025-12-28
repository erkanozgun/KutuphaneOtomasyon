using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kutuphane.Application.Dtos.ReportDtos
{
    public class AdminStatsDto
    {
        // Mevcut alanlar
        public List<string> Months { get; set; } = new();
        public List<int> MonthlyLoanCounts { get; set; } = new();
        public List<int> MonthlyReturnCounts { get; set; } = new(); // YENİ
        public List<string> Categories { get; set; } = new();
        public List<int> BookCountsByCategory { get; set; } = new();
        public List<TopReaderDto> TopReaders { get; set; } = new();
        public List<string> TopAuthors { get; set; } = new();
        public List<int> BookCountsByAuthor { get; set; } = new();

        // Özet istatistikler
        public int TotalBooks { get; set; }
        public int TotalMembers { get; set; }
        public int ActiveLoans { get; set; }
        public int TotalCopies { get; set; } // YENİ
        public int AvailableCopies { get; set; } // YENİ
        public int OverdueLoans { get; set; } // YENİ
        public int TotalLoansThisMonth { get; set; } // YENİ
        public int TotalLoansLastMonth { get; set; } // YENİ
        public int NewMembersThisMonth { get; set; } // YENİ
        public int NewBooksThisMonth { get; set; } // YENİ

        // Popüler kitaplar
        public List<PopularBookDto> PopularBooks { get; set; } = new(); // YENİ

        // Haftalık aktivite (son 7 gün)
        public List<string> WeekDays { get; set; } = new(); // YENİ
        public List<int> DailyLoanCounts { get; set; } = new(); // YENİ

        // Yüzdelik oranlar
        public double AvailabilityRate => TotalCopies > 0 ? Math.Round((double)AvailableCopies / TotalCopies * 100, 1) : 0;
        public double ActiveMemberRate => TotalMembers > 0 ? Math.Round((double)ActiveLoans / TotalMembers * 100, 1) : 0;
        public int LoanGrowthPercent => TotalLoansLastMonth > 0
            ? (int)Math.Round((double)(TotalLoansThisMonth - TotalLoansLastMonth) / TotalLoansLastMonth * 100)
            : 0;
    }

    public class TopReaderDto
    {
        public string MemberName { get; set; } = string.Empty;
        public string MemberNumber { get; set; } = string.Empty;
        public int TotalReadCount { get; set; }
    }
}
