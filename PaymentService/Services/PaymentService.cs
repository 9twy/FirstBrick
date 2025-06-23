using MassTransit;
using RabbitMQ.Client;
using System.Security.Claims;

public class PaymentServices : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public PaymentServices(IPaymentRepository paymentRepository, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _paymentRepository = paymentRepository;
    }
    public async Task CreateWalletAsync(int userId)
    {
        await _paymentRepository.CreateWalletAsync(userId);
    }
    public async Task<BalnaceDto> GetBalnaceAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var Id = int.Parse(user!.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var wallet = await _paymentRepository.GetWallet(Id);
        var walletDto = new BalnaceDto(wallet.Balance);
        return walletDto;


    }
    public async Task ApplyPayTopUpAsync(TopupMockDto topupMockDto)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var Id = int.Parse(user!.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _paymentRepository.PopUp(Id, topupMockDto.Amount);
    }

    public async Task<PaginatedList<Transaction>> GetTransactionsAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var Id = int.Parse(user!.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var paginatedList = await _paymentRepository.GetTransactionsAsync(Id);
        return paginatedList;

    }
    public async Task<bool> MakePaymentAsync(int userId, decimal amount, int projectId)
    {
        var result = await _paymentRepository.MakePaymentAsync(userId, amount, projectId);
        return result;

    }

}