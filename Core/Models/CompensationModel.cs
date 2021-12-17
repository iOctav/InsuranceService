namespace Core.Models;

public class CompensationModel
{
    public Guid? CompensationId { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string? Verdict { get; set; }
    public string? OperatorFullName { get; set; }
    public string CompanyName { get; set; }
    public decimal Amount { get; set; }
    public decimal InsuranceAmount { get; set; }
    public DateTime AppliedOn { get; set; }
    public string? Description { get; set; }
    public RequestState Status { get; set; }
    public Guid? ConsumerId { get; set; }
}