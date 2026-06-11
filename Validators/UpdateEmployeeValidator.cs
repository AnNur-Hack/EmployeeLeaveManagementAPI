using FluentValidation;
using EmployeeLeaveManagementAPI.DTOs;
using EmployeeLeaveManagementAPI.Constants;
namespace EmployeeLeaveManagementAPI.Validators;

public class UpdateEmployeeValidator :  AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name cannot be empty")
            .MinimumLength(3)
            .WithMessage("Full name must be at least 3 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Please provide a valid email address");

        RuleFor(x => x.Department)
            .NotEmpty()
            .WithMessage("Department cannot be empty")
            .Must(BeValidDepartment)
            .WithMessage($"Department must be one of: {string.Join(", ", DepartmentConstants.Departments)}");
    }

    private bool BeValidDepartment(string department)
    {
        return DepartmentConstants.Departments.Contains(department);
    }
}