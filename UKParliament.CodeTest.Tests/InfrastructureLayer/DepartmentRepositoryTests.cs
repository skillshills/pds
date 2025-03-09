using Moq;
using UKParliament.CodeTest.Domain.Models;
using UKParliament.CodeTest.Domain.Repositories;
using Xunit;

namespace UKParliament.CodeTest.Tests.InfrastructureLayer;

public class DepartmentRepositoryTests
{
    private readonly Mock<IDepartmentRepository> _departmentRepositoryMock = new Mock<IDepartmentRepository>();

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldReturnDepartment()
    {
        // Arrange
        var departmentId = 1;
        var expectedDepartment = new Department { Id = departmentId, Name = "HR" };
        _departmentRepositoryMock.Setup(service => service.GetDepartmentByIdAsync(departmentId)).ReturnsAsync(expectedDepartment);

        // Act
        var actualDepartment = await _departmentRepositoryMock.Object.GetDepartmentByIdAsync(departmentId);

        // Assert
        Assert.Equal(expectedDepartment, actualDepartment);
    }

    [Fact]
    public async Task GetDepartmentCountAsync_ShouldReturnDepartmentTotal()
    {
        // Arrange
        var expectedCount = 3;
        _departmentRepositoryMock.Setup(service => service.GetDepartmentTotalAsync()).ReturnsAsync(expectedCount);

        // Act
        var actualCount = await _departmentRepositoryMock.Object.GetDepartmentTotalAsync();

        // Assert
        Assert.Equal(expectedCount, actualCount);
    }

    [Fact]
    public async Task ListDepartmentsAsync_ShouldReturnListOfDepartments()
    {
        // Arrange
        var expectedDepartments = new List<Department>
        {
            new Department { Id = 1, Name = "HR" },
            new Department { Id = 2, Name = "IT" },
            new Department { Id = 3, Name = "Accounts" }
        };
        _departmentRepositoryMock.Setup(service => service.ListDepartmentsAsync()).ReturnsAsync(expectedDepartments);

        // Act
        var actualDepartments = await _departmentRepositoryMock.Object.ListDepartmentsAsync();

        // Assert
        Assert.Equal(expectedDepartments, actualDepartments);
    }
}