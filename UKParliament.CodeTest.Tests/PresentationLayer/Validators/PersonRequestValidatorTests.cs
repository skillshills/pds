using FluentValidation.TestHelper;
using Moq;
using UKParliament.CodeTest.Domain.Models;
using UKParliament.CodeTest.Domain.Services;
using UKParliament.CodeTest.Domain.ViewModels;
using UKParliament.CodeTest.Web.Validators;

using Xunit;

namespace UKParliament.CodeTest.Tests.PresentationLayer.Validators;

public class PersonRequestValidatorTests
{
    private readonly PersonRequestValidator _validator;
    private readonly Mock<IDepartmentService> _departmentServiceMock;

    public PersonRequestValidatorTests()
    {
        _departmentServiceMock = new Mock<IDepartmentService>();
        _validator = new PersonRequestValidator(_departmentServiceMock.Object);
    }

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_NullOrEmpty()
    {
        // Arrange
        var model = new PersonViewModel { FirstName = "" };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_LastName_Is_NullOrEmpty()
    {
        // Arrange
        var model = new PersonViewModel { LastName = "" };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Should_Have_Error_When_DateOfBirth_Is_Default()
    {
        // Arrange
        var model = new PersonViewModel { DateOfBirth = default };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
              .WithErrorMessage("Date of birth is required");
    }

    [Fact]
    public void Should_Have_Error_When_DateOfBirth_Is_In_The_Future()
    {
        // Arrange
        var model = new PersonViewModel { DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void Should_Not_Have_Error_When_DateOfBirth_Is_Valid_And_In_The_Past()
    {
        // Arrange
        var model = new PersonViewModel { DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void Should_Have_Error_When_DepartmentId_Is_Zero_Or_Less()
    {
        // Arrange
        var model = new PersonViewModel { DepartmentId = 0 };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DepartmentId);
    }

    [Fact]
    public async Task Should_Have_Error_When_Department_Does_Not_Exist()
    {
        // Arrange
        _departmentServiceMock.Setup(x => x.GetDepartmentByIdAsync(It.IsAny<int>())).ReturnsAsync((Department)null);
        
        var model = new PersonViewModel { DepartmentId = 1 };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DepartmentId);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Department_Exists()
    {
        // Arrange
        _departmentServiceMock.Setup(x => x.GetDepartmentByIdAsync(It.IsAny<int>())).ReturnsAsync(new Department());

        var model = new PersonViewModel { DepartmentId = 1 };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.DepartmentId);
    }
}