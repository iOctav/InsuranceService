namespace Core.Models;

public class DiseaseHistory : Request
{
    public Consumer Consumer { get; set; }
    public Operator Controller { get; set; }
    public string? Description { get; set;  }
}