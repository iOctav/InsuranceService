namespace Core.Models;

public abstract class Request
{
    public Guid Id { get; set; }
    public RequestState State { get; set; }
    public DateTime Date { get; set; }
}

public enum RequestState
{
    Open,
    Processing,
    Approved,
    Closed
}