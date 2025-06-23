public interface IInvestmentService
{
    public Task<CreateProjectDto> CreateProjectAsync(CreateProjectDto dto);
    public int? GetUserId();
    public Task<List<ProjectDto>> GetProjectsAsync();
    public Task ContributeToProject(ContributeToProjectDto contributeToProjectDto);
    public Task<List<Investment>> GetUserPortfolioAsync();
    public Task<Investment> GetUserPortfolioByIdAsync(int portfolioId);

}