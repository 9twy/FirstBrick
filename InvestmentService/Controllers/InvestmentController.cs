using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
[ApiController]
[Route("v1/[controller]")]
public class InvestmentController : ControllerBase
{
    private readonly IInvestmentService _investmentService;
    public InvestmentController(IInvestmentService investmentService)
    {
        _investmentService = investmentService;
    }
    [HttpPost("project")]
    [Authorize]
    public async Task<IActionResult> createProjectAsync(CreateProjectDto dto)
    {
        // Assuming _investmentService is injected and available
        var responseDto = await _investmentService.CreateProjectAsync(dto);
        return Created();
    }
    [HttpGet("projects")]
    [Authorize]
    public async Task<IActionResult> GetProjectsAsync()
    {
        var projects = await _investmentService.GetProjectsAsync();
        return Ok(projects);

    }
    [HttpPost("invest")]
    [Authorize]
    public async Task<IActionResult> ContributeToProjectAsync(ContributeToProjectDto contributeToProjectDto)
    {
        await _investmentService.ContributeToProject(contributeToProjectDto);
        return Ok();
    }
    [HttpGet("portfolio")]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolioAsync()
    {
        var investments = await _investmentService.GetUserPortfolioAsync();
        return Ok(investments);
    }
    [HttpGet("portfolio/{portfolioId:int}")]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolioByIdAsync(int portfolioId)
    {
        var investment = await _investmentService.GetUserPortfolioByIdAsync(portfolioId);
        return Ok(investment);
    }


}