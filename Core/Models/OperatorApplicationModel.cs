namespace Core.Models;

public class OperatorApplicationModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string OperatorFullName { get; set; }
    public decimal Premium { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime Date { get; set; }
}