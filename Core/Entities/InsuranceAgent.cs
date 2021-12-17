namespace Core.Models;

public class InsuranceAgent
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public IEnumerable<InsuranceContract> InsuranceContracts { get; set; }
    public decimal Tariff { get; set; }
    public Credential Credential { get; set; }
}