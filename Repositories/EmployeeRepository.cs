using EmployeeLeaveManagementAPI.Data;
using EmployeeLeaveManagementAPI.DTOs;
using EmployeeLeaveManagementAPI.Models;
using Microsoft.EntityFrameworkCore;





namespace EmployeeLeaveManagementAPI.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
     private readonly ApplicationDbContext _dbContext;

    public EmployeeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<EmployeeResponseDto>> GetAllEmployees()
    {
        var employees = await _dbContext.Employees.ToListAsync();
        
        if (employees.Count == 0)
        {
            throw new Exception("No employees found");
        }

        var result = new List<EmployeeResponseDto>();
        foreach (var emp in employees)
        {
            result.Add(new EmployeeResponseDto
            {
                Id = emp.Id,
                FullName = emp.FullName,
                Email = emp.Email,
                Department = emp.Department,
                DateJoined = emp.DateJoined
            });
        }
        return result;
    }

    public async Task<EmployeeResponseDto> GetEmployeeById(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);

        if (employee == null)
        {
            throw new Exception($"Employee with Id {id} not found");
        }

        return new EmployeeResponseDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Email = employee.Email,
            Department = employee.Department,
            DateJoined = employee.DateJoined
        };
    }

    public async Task<EmployeeResponseDto> CreateEmployee(CreateEmployeeDto dto)
    {
        var emailExists = await _dbContext.Employees.AnyAsync(x => x.Email == dto.Email);
        if (emailExists)
        {
            throw new Exception("Email already exists");
        }

        var employee = new Employee
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Department = dto.Department,
            DateJoined = DateTime.UtcNow
        };

        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        return new EmployeeResponseDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Email = employee.Email,
            Department = employee.Department,
            DateJoined = employee.DateJoined
        };
    }

    public async Task<EmployeeResponseDto> UpdateEmployee(int id, UpdateEmployeeDto dto)
    {
        var employee = await _dbContext.Employees.FindAsync(id);
        
        if (employee == null)
        {
            throw new Exception($"Employee with Id {id} not found");
        }

        employee.FullName = dto.FullName;
        employee.Email = dto.Email;
        employee.Department = dto.Department;

        await _dbContext.SaveChangesAsync();

        return new EmployeeResponseDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Email = employee.Email,
            Department = employee.Department,
            DateJoined = employee.DateJoined
        };
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);
        
        if (employee == null)
        {
            throw new Exception($"Employee with Id {id} not found");
        }

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }

    public async Task<List<LeaveResponseDto>> GetEmployeeLeaveHistory(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);
        if (employee == null)
        {
            throw new Exception($"Employee with Id {id} not found");
        }

        var leaves = await _dbContext.LeaveRequests
            .Include(x => x.Employee)
            .Include(x => x.LeaveApprovals)
            .ThenInclude(x => x.Approver)
            .Where(x => x.EmployeeId == id)
            .ToListAsync();

        var result = new List<LeaveResponseDto>();
        foreach (var leave in leaves)
        {
            result.Add(MapToLeaveResponse(leave));
        }
        return result;
    }

    public async Task<List<EmployeeResponseDto>> GetEmployeesCurrentlyOnLeave()
    {
        var today = DateTime.Today;
        
        var employees = await _dbContext.Employees
            .Where(x => x.LeaveRequests.Any(l => 
                l.Status == "Approved" && 
                today >= l.StartDate && 
                today <= l.EndDate))
            .ToListAsync();

        var result = new List<EmployeeResponseDto>();
        foreach (var emp in employees)
        {
            result.Add(new EmployeeResponseDto
            {
                Id = emp.Id,
                FullName = emp.FullName,
                Email = emp.Email,
                Department = emp.Department,
                DateJoined = emp.DateJoined
            });
        }
        return result;
    }

    private LeaveResponseDto MapToLeaveResponse(LeaveRequest leave)
    {
        var approvals = new List<ApprovalResponseDto>();
        foreach (var approval in leave.LeaveApprovals)
        {
            approvals.Add(new ApprovalResponseDto
            {
                Id = approval.Id,
                ApproverId = approval.ApproverId,
                ApproverName = approval.Approver?.FullName,
                Action = approval.Action,
                Reason = approval.Reason,
                DateActed = approval.DateActed
            });
        }

        return new LeaveResponseDto
        {
            Id = leave.Id,
            EmployeeId = leave.EmployeeId,
            EmployeeName = leave.Employee?.FullName,
            LeaveType = leave.LeaveType,
            StartDate = leave.StartDate,
            EndDate = leave.EndDate,
            Reason = leave.Reason,
            Status = leave.Status,
            DateCreated = leave.DateCreated,
            Approvals = approvals
        };
    }
}