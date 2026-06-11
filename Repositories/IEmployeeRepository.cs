using EmployeeLeaveManagementAPI.DTOs;

namespace EmployeeLeaveManagementAPI.Repositories;

public interface IEmployeeRepository
{
    Task<List<EmployeeResponseDto>> GetAllEmployees();
    Task<EmployeeResponseDto> GetEmployeeById(int id);
    Task<EmployeeResponseDto> CreateEmployee(CreateEmployeeDto dto);
    Task<EmployeeResponseDto> UpdateEmployee(int id, UpdateEmployeeDto dto);
    Task<bool> DeleteEmployee(int id);
    Task<List<LeaveResponseDto>> GetEmployeeLeaveHistory(int id);
    Task<List<EmployeeResponseDto>> GetEmployeesCurrentlyOnLeave();
}