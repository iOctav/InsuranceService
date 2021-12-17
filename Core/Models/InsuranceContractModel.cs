namespace Core.Models;

public class InsuranceContractModel
{
    public Guid InsuranceContractId { get; set; } 
    public Guid ConsumerId { get; set; }
    public Guid InsuranceAgentId { get; set; }
    public string AgentCompanyName{ get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IEnumerable<CompensationModel> Compensations { get; set; }
    public decimal InsurancePremium { get; set; }
    public decimal InsuranceAmount { get; set; }
    public InsuranceContractState State { get; set; }
}

public enum InsuranceContractState
{
    Created,
    Processing,
    Approved,
    Rejected
}