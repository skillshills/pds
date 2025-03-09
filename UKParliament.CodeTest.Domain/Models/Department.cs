namespace UKParliament.CodeTest.Domain.Models;

public class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Person>? People { get; set; }
}
