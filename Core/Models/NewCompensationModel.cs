namespace Core.Models;

public class NewCompensationModel
{
    public Guid? ConsumerId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}