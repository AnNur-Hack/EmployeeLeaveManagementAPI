using EmployeeLeaveManagementAPI.Models;
namespace EmployeeLeaveManagementAPI.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllEmployees();

    Task<Employee> GetEmployeeById(int id);

    Task<Employee> CreateEmployee(CreateEmployeeDtos dto);

    Task<Employee> UpdateEmployee(Employee employee);

    Task<bool> DeleteEmployee(int id);

    Task<IEnumerable<LeaveRequest>> GetEmployeeLeaveHistory(int id);
}