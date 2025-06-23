public interface IPaymentRepository
{
    Task CreateWalletAsync(int userId);
    Task<Wallet> GetWallet(int userId);
    Task PopUp(int userId, decimal balance);
    Task<PaginatedList<Transaction>> GetTransactionsAsync(int userId, int pageIndex = 1, int pageSize = 10);
    Task<bool> MakePaymentAsync(int UserId, decimal Amount, int projectId);
}