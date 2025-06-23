using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
[ApiController]
[Route("v1/[controller]")]

public class PaymentController : ControllerBase
{
    private readonly IPaymentService _service;
    public PaymentController(IPaymentService service)
    {
        _service = service;
    }
    [HttpPost("ApplepayTopup")]
    [Authorize]
    public async Task<IActionResult> ApplyPayTopUpAsync(TopupMockDto topupMockDto)
    {
        await _service.ApplyPayTopUpAsync(topupMockDto);
        return Ok();
    }
    [HttpGet("balance")]
    [Authorize]
    public async Task<IActionResult> GetBalnaceAsync()
    {
        var walletDto = await _service.GetBalnaceAsync();
        return Ok(walletDto);
    }
    [HttpGet("transactions")]
    [Authorize]
    public async Task<ActionResult<ApiResponse>> GetTransactionsAsync()
    {
        var transactions = await _service.GetTransactionsAsync();
        return new ApiResponse(true, string.Empty, transactions);
    }
}