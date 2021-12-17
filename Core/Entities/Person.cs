namespace Core.Models;

public abstract class Person
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Surname { get; set; }
    public Credential Credential { get; set; }
}