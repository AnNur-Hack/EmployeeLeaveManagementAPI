namespace EmployeeLeaveManagementAPI.DTOs;

public class LeaveStatisticsDto
{
    public string Department { get; set; }
    public int TotalLeaveRequests { get; set; }
    public int ApprovedRequests { get; set; }
    public int RejectedRequests { get; set; }
    public int PendingRequests { get; set; }
}