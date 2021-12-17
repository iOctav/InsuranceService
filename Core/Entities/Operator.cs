namespace Core.Models;

public class Operator : Person
{
    public IEnumerable<Application> Applications { get; set; }
    public IEnumerable<Compensation> Compensations { get; set; }
    public IEnumerable<DiseaseHistory> DiseaseHistories { get; set; }
}