using EmployeeLeaveManagementAPI.Models;

namespace EmployeeLeaveManagementAPI.Repositories;

public interface ILeaveRepository
{
    Task<IEnumerable<LeaveRequest>> GetAllLeaves();

    Task<LeaveRequest> GetLeaveById(int id);

    Task<LeaveRequest> CreateLeave(CreateLeaveRequestDtos dto);

    Task<LeaveRequest> UpdateLeave(LeaveRequest leave);

    Task<bool> DeleteLeave(int id);

    Task<LeaveRequest> ApproveLeave(int id);

    Task<LeaveRequest> RejectLeave(int id);

    Task<IEnumerable<LeaveRequest>> FilterByStatus(string status);

    Task<IEnumerable<Employee>> GetEmployeesCurrentlyOnLeave();
}