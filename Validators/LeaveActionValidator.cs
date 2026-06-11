using EmployeeLeaveManagementAPI.DTOs;
using FluentValidation;


namespace EmployeeLeaveManagementAPI.Validators;

public class LeaveActionValidator : AbstractValidator<LeaveActionDto>
{
    public LeaveActionValidator()
    {
        RuleFor(x => x.ApproverId)
            .GreaterThan(0)
            .WithMessage("Approver ID must be greater than 0");
    }
}