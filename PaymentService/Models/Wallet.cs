public class Wallet
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string AccountNumber { get; set; } = string.Concat(Enumerable.Range(0, 10).Select(_ => new Random().Next(0, 10)));
    public decimal Balance { get; set; } = 0.0m;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}