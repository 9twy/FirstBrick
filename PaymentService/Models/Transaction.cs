public class Transaction
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int WalletId { get; set; }
    public virtual Wallet? Wallet { get; set; }

    public string? PaymentMethod { get; set; } = "Apple Pay";
    public string PaymentType { get; set; } = "TopUp";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}