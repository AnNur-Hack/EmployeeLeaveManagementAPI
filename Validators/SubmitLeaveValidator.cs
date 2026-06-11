using EmployeeLeaveManagementAPI.DTOs;
using EmployeeLeaveManagementAPI.Constants;
using FluentValidation;


namespace EmployeeLeaveManagementAPI.Validators;

public class SubmitLeaveValidator : AbstractValidator<SubmitLeaveDto>
{
    public SubmitLeaveValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("Employee ID must be greater than 0");

        RuleFor(x => x.LeaveType)
            .NotEmpty()
            .WithMessage("Leave type cannot be empty")
            .Must(BeValidLeaveType)
            .WithMessage($"Leave type must be one of: {string.Join(", ", LeaveTypeConstants.LeaveTypes)}");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Start date cannot be in the past");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be greater than or equal to start date");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Reason cannot be empty")
            .MinimumLength(10)
            .WithMessage("Reason must be at least 10 characters")
            .MaximumLength(500)
            .WithMessage("Reason cannot exceed 500 characters");
    }

    private bool BeValidLeaveType(string leaveType)
    {
        return LeaveTypeConstants.LeaveTypes.Contains(leaveType.ToUpper());
    }
}