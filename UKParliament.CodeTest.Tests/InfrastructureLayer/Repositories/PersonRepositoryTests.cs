using Moq;
using UKParliament.CodeTest.Domain.Models;
using UKParliament.CodeTest.Domain.Repositories;
using Xunit;

namespace UKParliament.CodeTest.Tests.InfrastructureLayer.Repositories;

public class PersonRepositoryTests
{
    private readonly Mock<IPersonRepository> _personRepositoryMock = new Mock<IPersonRepository>();

    [Fact]
    public async Task CreatePersonAsync_ShouldCreatePerson()
    {
        // Arrange
        var newPerson = new Person { FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1990, 1, 1), DepartmentId = 1 };
        var expectedPerson = new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1990, 1, 1), DepartmentId = 1 };
        _personRepositoryMock.Setup(service => service.CreatePersonAsync(newPerson)).ReturnsAsync(expectedPerson);

        // Act
        await _personRepositoryMock.Object.CreatePersonAsync(newPerson);

        // Assert
        _personRepositoryMock.Verify(service => service.CreatePersonAsync(newPerson), Times.Once);
    }

    [Fact]
    public async Task GetPersonTotalAsync_ShouldReturnPersonTotal()
    {
        // Arrange
        var expectedCount = 5;
        _personRepositoryMock.Setup(service => service.GetPersonTotalAsync()).ReturnsAsync(expectedCount);

        // Act
        var actualCount = await _personRepositoryMock.Object.GetPersonTotalAsync();

        // Assert
        Assert.Equal(expectedCount, actualCount);
    }

    [Fact]
    public async Task GetPersonListAsync_ShouldReturnListOfPeople()
    {
        // Arrange
        var expectedPeople = new List<Person>
        {
            new Person { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1990, 1, 1), DepartmentId = 1 },
            new Person { Id = 2, FirstName = "Jane", LastName = "Doe", DateOfBirth = new DateOnly(1992, 2, 2), DepartmentId = 2 }
        };
        _personRepositoryMock.Setup(service => service.GetPersonListAsync()).ReturnsAsync(expectedPeople);

        // Act
        var actualPeople = await _personRepositoryMock.Object.GetPersonListAsync();

        // Assert
        Assert.Equal(expectedPeople, actualPeople);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ShouldReturnPerson()
    {
        // Arrange
        var personId = 1;
        var expectedPerson = new Person { Id = personId, FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1990, 1, 1), DepartmentId = 1 };
        _personRepositoryMock.Setup(service => service.GetPersonByIdAsync(personId)).ReturnsAsync(expectedPerson);

        // Act
        var actualPerson = await _personRepositoryMock.Object.GetPersonByIdAsync(personId);

        // Assert
        Assert.Equal(expectedPerson, actualPerson);
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldUpdatePerson()
    {
        // Arrange
        var personId = 1;
        var updatedPerson = new Person { Id = personId, FirstName = "John", LastName = "Doe", DateOfBirth = new DateOnly(1990, 1, 1), DepartmentId = 1 };
        _personRepositoryMock.Setup(service => service.UpdatePersonAsync(personId, updatedPerson)).Returns(Task.CompletedTask);

        // Act
        await _personRepositoryMock.Object.UpdatePersonAsync(personId, updatedPerson);

        // Assert
        _personRepositoryMock.Verify(service => service.UpdatePersonAsync(personId, updatedPerson), Times.Once);
    }

    [Fact]
    public async Task DeletePersonAsync_ShouldDeletePerson()
    {
        // Arrange
        var personId = 1;
        _personRepositoryMock.Setup(service => service.DeletePersonAsync(personId)).Returns(Task.CompletedTask);

        // Act
        await _personRepositoryMock.Object.DeletePersonAsync(personId);

        // Assert
        _personRepositoryMock.Verify(service => service.DeletePersonAsync(personId), Times.Once);
    }
}
