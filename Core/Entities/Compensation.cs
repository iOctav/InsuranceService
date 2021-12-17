namespace Core.Models;

public class Compensation : Request
{
    public Operator? Controller { get; set; }
    public string? Verdict { get; set; }
    public InsuranceContract InsuranceContract { get; set; }
    public string? Description { get; set; }
    public decimal? Amount { get; set; }
}