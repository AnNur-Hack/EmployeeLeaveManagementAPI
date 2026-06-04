using EmployeeLeaveManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using EmployeeLeaveManagementAPI.Models;

namespace EmployeeLeaveManagementAPI.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
     private readonly ApplicationDbContext _dbContext;

    public EmployeeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        return await _dbContext.Employees.ToListAsync();
    }

    public async Task<Employee> GetEmployeeById(int id)
    {
        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(x => x.Id == id);

        if (employee == null)
        {
            throw new Exception($"Employee with Id {id} not found");
        }

        return employee;
    }

    public async Task<Employee> CreateEmployee(CreateEmployeeDtos dto)
    {
        var emailExist = await _dbContext.Employees
            .AnyAsync(x => x.Email == dto.Email);

        if (emailExist)
        {
            throw new Exception("Employee email already exists");
        }

        var employee = new Employee
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Department = dto.Department,
            DateJoined = DateTime.Now
        };

        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        return employee;
    }

    public async Task<Employee> UpdateEmployee(Employee employee)
    {
        var employeeExist = await _dbContext.Employees
            .FirstOrDefaultAsync(x => x.Id == employee.Id);

        if (employeeExist == null)
        {
            throw new Exception("Employee not found");
        }

        employeeExist.FullName = employee.FullName;
        employeeExist.Email = employee.Email;
        employeeExist.Department = employee.Department;

        await _dbContext.SaveChangesAsync();

        return employeeExist;
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(x => x.Id == id);

        if (employee == null)
        {
            throw new Exception("Employee not found");
        }

        _dbContext.Employees.Remove(employee);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<LeaveRequest>> GetEmployeeLeaveHistory(int id)
    {
        return await _dbContext.LeaveRequests
            .Where(x => x.EmployeeId == id)
            .ToListAsync();
    }
}