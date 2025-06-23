using Microsoft.EntityFrameworkCore;

public class InvestmentDbContext : DbContext
{
    public InvestmentDbContext(DbContextOptions<InvestmentDbContext> options) : base(options)
    { }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Investment> investments { get; set; }
}
