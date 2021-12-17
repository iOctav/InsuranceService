namespace Core.Models;

public class Credential
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string Password { get; set; }
    public bool IsBlocked { get; set; }
}