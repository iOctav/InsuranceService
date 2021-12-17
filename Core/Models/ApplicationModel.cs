namespace Core.Models;

public class ApplicationModel
{
    public Guid ApplicationId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal InsurancePremium { get; set; }
    public decimal InsuranceAmount { get; set; }
    public InsuranceContractState State { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid ConsumerId { get; set; }
    public Guid? OperatorId { get; set; }
    public Guid AgentId { get; set; }
}