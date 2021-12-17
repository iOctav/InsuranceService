namespace Core.Models;

public class InsuranceContract
{
    public Guid Id { get; set; }
    public Consumer Consumer { get; set; }
    public InsuranceAgent InsuranceAgent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IEnumerable<Compensation>? Compensations { get; set; }
    public decimal InsurancePremium { get; set; }
    public decimal InsuranceAmount { get; set; }
}