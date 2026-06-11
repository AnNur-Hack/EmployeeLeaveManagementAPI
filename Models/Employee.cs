namespace EmployeeLeaveManagementAPI.Models;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
    public DateTime DateJoined { get; set; }

    public ICollection<LeaveRequest> LeaveRequests { get; set; }
    public ICollection<LeaveApproval> LeaveApprovals { get; set; }

    public Employee()
    {
        LeaveRequests = new List<LeaveRequest>();
        LeaveApprovals = new List<LeaveApproval>();
    }
}