using System.Security.Claims;

public class InvestmentServices : IInvestmentService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IInvestmentRepositories _investmentRepository;
    private readonly GrpcPaymentClient.MakePaymentService.MakePaymentServiceClient _paymentClient;
    public InvestmentServices(IInvestmentRepositories investmentRepository, IHttpContextAccessor httpContextAccessor, GrpcPaymentClient.MakePaymentService.MakePaymentServiceClient paymentClient)
    {
        _paymentClient = paymentClient;
        _httpContextAccessor = httpContextAccessor;
        _investmentRepository = investmentRepository;
    }
    public async Task<CreateProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto)
    {
        if (createProjectDto == null)
        {
            throw new Exception("Project data cannot be null.");
        }

        var project = new Project
        {

            UserId = GetUserId() ?? throw new Exception("User ID cannot be null."),
            Name = createProjectDto.Name,
            Description = createProjectDto.Description,
            Location = createProjectDto.Location,
            NumberOfUnits = createProjectDto.NumberOfUnits,
            UnitPrice = createProjectDto.UnitPrice,
            StartDate = createProjectDto.StartDate,
            EndDate = createProjectDto.EndDate

        };
        await _investmentRepository.CreateProjectAsync(project);
        return createProjectDto;

    }
    public int? GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return int.Parse(user!.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    public async Task<List<ProjectDto>> GetProjectsAsync()
    {
        var projects = await _investmentRepository.GetProjectsAsync();
        return projects.Select(p => new ProjectDto(
            p.Id,
            p.UserId,
            p.Description,
            p.Name,
            p.Location,
            p.NumberOfUnits,
            p.UintsFilled,
            p.UnitPrice,
            p.StartDate,
            p.EndDate
        )).ToList();
    }
    public async Task ContributeToProject(ContributeToProjectDto contributeToProjectDto)
    {

        int userId = GetUserId() ?? throw new UnauthorizedAccessException("User is not authorized.");


        var project = await _investmentRepository.GetProjectAsync(contributeToProjectDto.ProjectId);
        if (project == null)
        {
            throw new Exception("Project not found.");
        }
        var freeUints = project.NumberOfUnits - project.UintsFilled;
        if (contributeToProjectDto.Units > freeUints)
        {
            throw new Exception("Uints Requested is more than the offered Uints");
        }


        var request = new GrpcPaymentClient.MakePaymentRequest
        {
            UserId = userId,
            Amount = project.UnitPrice * contributeToProjectDto.Units,
            ProjectId = project.Id

        };


        var response = await _paymentClient.MakePaymentAsync(request);
        Console.WriteLine($"Response states: {response.Success}");
        if (!response.Success)
        {
            throw new Exception($"Payment failed: {response.Message}");
        }


        await _investmentRepository.ContributeToProject(userId, contributeToProjectDto);
    }
    public async Task<List<Investment>> GetUserPortfolioAsync()
    {
        int userId = GetUserId() ?? throw new UnauthorizedAccessException("User is not authorized.");
        return await _investmentRepository.GetUserPortfolioAsync(userId);
    }
    public async Task<Investment> GetUserPortfolioByIdAsync(int portfolioId)
    {
        int userId = GetUserId() ?? throw new UnauthorizedAccessException("User is not authorized.");
        return await _investmentRepository.GetUserPortfolioByIdAsync(userId, portfolioId);
    }

}

