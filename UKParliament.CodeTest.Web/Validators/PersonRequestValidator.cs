using FluentValidation;
using UKParliament.CodeTest.Domain.Services;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Validators;

public class PersonRequestValidator : AbstractValidator<PersonViewModel>
{
    public PersonRequestValidator(IDepartmentService departmentService)
    {
        RuleFor(x => x.FirstName)
            .Must(NotBeNullOrEmpty).WithMessage("First name is required");

        RuleFor(x => x.LastName)
            .Must(NotBeNullOrEmpty).WithMessage("Last name is required");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required");

        RuleFor(x => x.DateOfBirth)
            .Must(BeInThePast).WithMessage("Date of birth date must be in the past");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("Department is required");

        RuleFor(x => x.DepartmentId)
            .MustAsync(async (departmentId, cancellation) =>
                (await departmentService.GetDepartmentByIdAsync(departmentId)) != null)
            .WithMessage("Department does not exist")
            .When(x => x.DepartmentId > 0);
    }

    private bool NotBeNullOrEmpty(string? value)
    {
        return !string.IsNullOrEmpty(value?.Replace(" ", "").Trim());
    }
    private bool BeInThePast(DateOnly date)
    {
        return date < DateOnly.FromDateTime(DateTime.Now);
    }
}
