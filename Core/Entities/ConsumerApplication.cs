namespace Core.Models;

public class ConsumerApplication
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string CompanyName { get; set; }
    public decimal Premium { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime Date { get; set; }
}