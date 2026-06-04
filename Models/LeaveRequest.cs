namespace EmployeeLeaveManagementAPI.Models;

public class LeaveRequest
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string LeaveType { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public DateTime DateCreated { get; set; }

    public Employee Employee { get; set; }
}