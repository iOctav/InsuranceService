namespace Core.Models;

public class NewApplicationModel
{
    public Guid? ConsumerId { get; set; }
    public Guid InsuranceAgentId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal InsurancePremium { get; set; }
    public decimal InsuranceAmount { get; set; }
}