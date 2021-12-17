using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.DbContexts;

public class InsuranceContext: DbContext
{
    public InsuranceContext() { }
        
    public DbSet<InsuranceContract> InsuranceContracts { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Consumer> Consumers { get; set; }
    public DbSet<Operator> Operators { get; set; }
    public DbSet<InsuranceAgent> InsuranceAgents { get; set; }
    public DbSet<Compensation> Compensations { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=InsuranceCompany.db");
    }
}