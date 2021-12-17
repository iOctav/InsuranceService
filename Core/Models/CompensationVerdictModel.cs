namespace Core.Models;

public class CompensationVerdictModel
{
    public Guid? CompensationId { get; set; }
    public string Verdict { get; set; }
    public bool Approved { get; set; }
    public decimal Amount { get; set; }
    public Guid? OperatorId { get; set; }
}