namespace UKParliament.CodeTest.Domain.ViewModels;

public class PersonViewModel
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }

    public int DepartmentId { get; set; }

    public string FullName => $"{FirstName} {LastName}".Trim();
}