namespace Core.Models;

public class Consumer : Person
{
    public string Phone { get; set; }
    public IEnumerable<InsuranceContract?> InsuranceContracts { get; set; }
    public IEnumerable<Compensation> Compensations { get; set; }
    public IEnumerable<DiseaseHistory> DiseaseHistories { get; set; }
}