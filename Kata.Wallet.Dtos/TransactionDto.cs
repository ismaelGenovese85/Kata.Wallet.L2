namespace Kata.Wallet.Dtos;

public class TransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public WalletDto WalletIncoming { get; set; } = null!;
    public WalletDto WalletOutgoing { get; set; } = null!;
}
