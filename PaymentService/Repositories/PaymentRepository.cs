using Microsoft.EntityFrameworkCore;
public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;
    public PaymentRepository(PaymentDbContext context)
    {
        _context = context;
    }
    public async Task CreateWalletAsync(int userId)
    {
        var wallet = new Wallet
        {
            UserId = userId

        };
        await _context.Wallets.AddAsync(wallet);
        await _context.SaveChangesAsync();

    }
    public async Task<Wallet> GetWallet(int userId)
    {
        return await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId) ?? throw new Exception("Invalid User Id");

    }
    public async Task PopUp(int userId, decimal balance)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId) ?? throw new Exception("invalid Id");
        wallet!.Balance += balance;
        _context.Update(wallet);
        await _context.SaveChangesAsync();

    }

    public async Task<PaginatedList<Transaction>> GetTransactionsAsync(int userId, int pageIndex = 1, int pageSize = 10)
    {
        // Step 1: Get the WalletId of the user (assume user has only 1 wallet)
        var walletId = await _context.Wallets
            .Where(w => w.UserId == userId)
            .Select(w => w.Id)
            .FirstOrDefaultAsync();

        if (walletId == 0)
            throw new Exception("Wallet not found");

        // Step 2: Query Transactions directly from the DbContext
        var query = _context.Transactions
            .Where(t => t.WalletId == walletId)
            .OrderByDescending(t => t.Id); // Optional: latest first

        var count = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(count / (double)pageSize);

        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<Transaction>(items, pageIndex, totalPages);
    }

    public async Task<bool> MakePaymentAsync(int UserId, decimal amount, int projectId)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(wallet => wallet.UserId == UserId);
        if (wallet!.Balance < amount)
        {
            return false;
        }
        wallet.Balance -= amount;
        var transaction = new Transaction
        {
            ProjectId = projectId,
            WalletId = wallet.Id


        };
        _context.Update(wallet);
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        return true;
    }

}