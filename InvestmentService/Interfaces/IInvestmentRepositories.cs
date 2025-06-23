public interface IInvestmentRepositories
{
    Task CreateProjectAsync(Project project);
    Task<List<Project>> GetProjectsAsync();
    Task ContributeToProject(int userId, ContributeToProjectDto contributeToProjectDto);
    Task<Project> GetProjectAsync(int projectId);
    Task<List<Investment>> GetUserPortfolioAsync(int userId);
    Task<Investment> GetUserPortfolioByIdAsync(int userId, int portfolioId);


}