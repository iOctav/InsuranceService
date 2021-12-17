namespace Core.Models;

public class Application : Request
{
    public Operator? Operator { get; set; }
    public Consumer? Consumer { get; set; }
    public InsuranceContract InsuranceContract { get; set; }
}