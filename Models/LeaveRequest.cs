namespace EmployeeLeaveManagementAPI.Models;

public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string LeaveType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
    public DateTime DateCreated { get; set; }
    public int ApprovalCount { get; set; }

    public Employee Employee { get; set; }
    public ICollection<LeaveApproval> LeaveApprovals { get; set; }
    
    
    public LeaveRequest()
    {
        LeaveApprovals = new List<LeaveApproval>();
        Status = "Pending";
        ApprovalCount = 0;
    }
}