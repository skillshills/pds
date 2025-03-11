using UKParliament.CodeTest.Domain.Models;
using UKParliament.CodeTest.Domain.ViewModels;

namespace UKParliament.CodeTest.Application.Extensions;

public static class Mapping
{
    public static DepartmentViewModel ToViewModel(this Department department)
    {
        return new DepartmentViewModel
        {
            Id = department.Id,
            Name = department.Name,
            TotalPeople = department.People?.Count ?? 0
        };
    }

    public static Department ToDataModel(this DepartmentViewModel department)
    {
        return new Department
        {
            Id = department.Id,
            Name = department.Name
        };
    }

    public static PersonViewModel ToViewModel(this Person person)
    {
        return new PersonViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId
        };
    }

    public static Person ToDataModel(this PersonViewModel person)
    {
        return new Person
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId
        };
    }
}
