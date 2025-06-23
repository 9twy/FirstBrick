using Microsoft.EntityFrameworkCore;

public class InvestmentRepository : IInvestmentRepositories
{
    private readonly InvestmentDbContext _context;
    public InvestmentRepository(InvestmentDbContext context)
    {
        _context = context;
    }
    public async Task CreateProjectAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

    }
    public async Task<List<Project>> GetProjectsAsync()
    {
        return await _context.Projects.ToListAsync();
    }
    public async Task<Project> GetProjectAsync(int projectId)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        return project!;
    }
    public async Task ContributeToProject(int userId, ContributeToProjectDto contributeToProjectDto)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == contributeToProjectDto.ProjectId);
        var investment = new Investment
        {
            UserId = userId,
            Project = project!,
            Units = contributeToProjectDto.Units,
            amount = contributeToProjectDto.Units * project!.UnitPrice
        };
        project.UintsFilled += contributeToProjectDto.Units;
        _context.Update(project);
        await _context.investments.AddAsync(investment);
        await _context.SaveChangesAsync();

    }
    public async Task<List<Investment>> GetUserPortfolioAsync(int userId)
    {
        var investments = await _context.investments
                        .Where(t => t.UserId == userId).ToListAsync();
        return investments;
    }
    public async Task<Investment> GetUserPortfolioByIdAsync(int userId, int portfolioId)
    {
        var investment = await _context.investments
                        .Where(i => i.UserId == userId).Where(i => i.Id == portfolioId).FirstOrDefaultAsync() ?? throw new Exception("Portioflio Not found");
        return investment!;
    }
}