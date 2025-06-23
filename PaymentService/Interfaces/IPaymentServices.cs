public interface IPaymentService
{
    public Task ApplyPayTopUpAsync(TopupMockDto topupMockDto);
    public Task CreateWalletAsync(int userId);
    public Task<BalnaceDto> GetBalnaceAsync();
    public Task<PaginatedList<Transaction>> GetTransactionsAsync();
    public Task<bool> MakePaymentAsync(int userId, decimal amount, int projectId);



}