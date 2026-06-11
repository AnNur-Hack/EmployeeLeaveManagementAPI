using EmployeeLeaveManagementAPI.DTOs;


namespace EmployeeLeaveManagementAPI.Repositories;

public interface ILeaveRepository
{
    Task<List<LeaveResponseDto>> GetAllLeaves();
    Task<LeaveResponseDto> GetLeaveById(int id);
    Task<LeaveResponseDto> CreateLeave(SubmitLeaveDto dto);
    Task<LeaveResponseDto> UpdateLeave(int id, SubmitLeaveDto dto);
    Task<bool> DeleteLeave(int id);
    Task<LeaveResponseDto> ApproveLeave(int id, LeaveActionDto dto);
    Task<LeaveResponseDto> RejectLeave(int id, LeaveActionDto dto);
    Task<List<LeaveResponseDto>> FilterByStatus(string status);
    Task<List<LeaveStatisticsDto>> GetLeaveStatistics();
}