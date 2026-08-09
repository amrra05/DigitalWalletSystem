namespace DigitalWallet.Application.DTOs.Admin;

public class AdminDashboardDto
{
    public int TotalUsers { get; set; }

    public int ActiveUsers { get; set; }

    public int FrozenUsers { get; set; }

    public int TotalTransactions { get; set; }

    public decimal TotalWalletBalance { get; set; }

    public List<RecentTransactionDto> RecentTransactions { get; set; } = new();
}