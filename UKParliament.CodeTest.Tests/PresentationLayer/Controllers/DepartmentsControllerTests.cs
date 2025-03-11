using Microsoft.AspNetCore.Mvc;
using Moq;
using UKParliament.CodeTest.Domain.Models;
using UKParliament.CodeTest.Domain.Services;
using UKParliament.CodeTest.Domain.ViewModels;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;

namespace UKParliament.CodeTest.Tests.PresentationLayer.Controllers;

public class DepartmentsControllerTests
{
    private readonly Mock<IDepartmentService> _departmentServiceMock;
    private readonly DepartmentsController _controller;

    public DepartmentsControllerTests()
    {
        _departmentServiceMock = new Mock<IDepartmentService>();
        _controller = new DepartmentsController(_departmentServiceMock.Object);
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldReturnDepartment_WhenDepartmentExists()
    {
        // Arrange
        var departmentId = 1;
        var expectedDepartment = new DepartmentViewModel { Id = departmentId, Name = "HR" };
        _departmentServiceMock.Setup(service => service.GetDepartmentByIdAsync(departmentId))
            .ReturnsAsync(new Department { Id = departmentId, Name = "HR" });

        // Act
        var result = await _controller.GetDepartmentByIdAsync(departmentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualDepartment = Assert.IsType<DepartmentViewModel>(okResult.Value);
        Assert.Equal(expectedDepartment.Id, actualDepartment.Id);
        Assert.Equal(expectedDepartment.Name, actualDepartment.Name);
    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
    {
        // Arrange
        var departmentId = 1;
        _departmentServiceMock.Setup(service => service.GetDepartmentByIdAsync(departmentId))
            .ReturnsAsync((Department)null);

        // Act
        var result = await _controller.GetDepartmentByIdAsync(departmentId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task ListDepartments_ShouldReturnListOfDepartments()
    {
        // Arrange
        var expectedDepartments = new List<DepartmentViewModel>
        {
            new DepartmentViewModel { Id = 1, Name = "HR" },
            new DepartmentViewModel { Id = 2, Name = "IT" }
        };
        _departmentServiceMock.Setup(service => service.ListDepartmentsAsync())
            .ReturnsAsync(new List<Department>
            {
                new Department { Id = 1, Name = "HR" },
                new Department { Id = 2, Name = "IT" }
            });

        // Act
        var result = await _controller.ListDepartments();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualDepartments = Assert.IsType<List<DepartmentViewModel>>(okResult.Value);
        Assert.Equal(expectedDepartments.Count, actualDepartments.Count);
        Assert.Equal(expectedDepartments[0].Id, actualDepartments[0].Id);
        Assert.Equal(expectedDepartments[0].Name, actualDepartments[0].Name);
        Assert.Equal(expectedDepartments[1].Id, actualDepartments[1].Id);
        Assert.Equal(expectedDepartments[1].Name, actualDepartments[1].Name);
    }

    [Fact]
    public async Task GetDepartmentTotalAsync_ShouldReturnDepartmentTotal()
    {
        // Arrange
        var expectedTotal = 5;
        _departmentServiceMock.Setup(service => service.GetDepartmentTotalAsync())
            .ReturnsAsync(expectedTotal);

        // Act
        var result = await _controller.GetDepartmentTotalAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualTotal = Assert.IsType<int>(okResult.Value.GetType().GetProperty("total").GetValue(okResult.Value, null));
        Assert.Equal(expectedTotal, actualTotal);
    }
}
