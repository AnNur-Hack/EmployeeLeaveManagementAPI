namespace EmployeeLeaveManagementAPI.Models;

public class Employee
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public DateTime DateJoined { get; set; }

    // One Employee -> Many Leave Requests
    public ICollection<LeaveRequest> LeaveRequests { get; set; }
        = new List<LeaveRequest>();
}