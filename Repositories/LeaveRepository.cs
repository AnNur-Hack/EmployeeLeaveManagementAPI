using EmployeeLeaveManagementAPI.Models;
using EmployeeLeaveManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
namespace EmployeeLeaveManagementAPI.Repositories;

public class LeaveRepository : ILeaveRepository
{
     private readonly ApplicationDbContext _dbContext;

    public LeaveRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<LeaveRequest>> GetAllLeaves()
    {
        return await _dbContext.LeaveRequests
            .Include(x => x.Employee)
            .ToListAsync();
    }

    public async Task<LeaveRequest> GetLeaveById(int id)
    {
        var leave = await _dbContext.LeaveRequests
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (leave == null)
        {
            throw new Exception("Leave request not found");
        }

        return leave;
    }

    public async Task<LeaveRequest> CreateLeave(CreateLeaveRequestDtos dto)
    {
        // Employee must exist

        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(x => x.Id == dto.EmployeeId);

        if (employee == null)
        {
            throw new Exception("Employee not found");
        }

        // Start Date <= End Date

        if (dto.StartDate > dto.EndDate)
        {
            throw new Exception("Start Date cannot be later than End Date");
        }

        // Overlapping Leave Check

        var overlap = await _dbContext.LeaveRequests.AnyAsync(x =>
            x.EmployeeId == dto.EmployeeId &&
            dto.StartDate <= x.EndDate &&
            dto.EndDate >= x.StartDate);

        if (overlap)
        {
            throw new Exception("Employee already has leave during this period");
        }

        var leave = new LeaveRequest
        {
            EmployeeId = dto.EmployeeId,
            LeaveType = dto.LeaveType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Reason = dto.Reason,
            Status = "Pending",
            DateCreated = DateTime.Now
        };

        await _dbContext.LeaveRequests.AddAsync(leave);
        await _dbContext.SaveChangesAsync();

        return leave;
    }

    public async Task<LeaveRequest> UpdateLeave(LeaveRequest leave)
    {
        var leaveExist = await _dbContext.LeaveRequests
            .FirstOrDefaultAsync(x => x.Id == leave.Id);

        if (leaveExist == null)
        {
            throw new Exception("Leave request not found");
        }

        leaveExist.LeaveType = leave.LeaveType;
        leaveExist.StartDate = leave.StartDate;
        leaveExist.EndDate = leave.EndDate;
        leaveExist.Reason = leave.Reason;

        await _dbContext.SaveChangesAsync();

        return leaveExist;
    }

    public async Task<bool> DeleteLeave(int id)
    {
        var leave = await _dbContext.LeaveRequests
            .FirstOrDefaultAsync(x => x.Id == id);

        if (leave == null)
        {
            throw new Exception("Leave request not found");
        }

        _dbContext.LeaveRequests.Remove(leave);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<LeaveRequest> ApproveLeave(int id)
    {
        var leave = await _dbContext.LeaveRequests
            .FirstOrDefaultAsync(x => x.Id == id);

        if (leave == null)
        {
            throw new Exception("Leave request not found");
        }

        leave.Status = "Approved";

        await _dbContext.SaveChangesAsync();

        return leave;
    }

    public async Task<LeaveRequest> RejectLeave(int id)
    {
        var leave = await _dbContext.LeaveRequests
            .FirstOrDefaultAsync(x => x.Id == id);

        if (leave == null)
        {
            throw new Exception("Leave request not found");
        }

        leave.Status = "Rejected";

        await _dbContext.SaveChangesAsync();

        return leave;
    }

    public async Task<IEnumerable<LeaveRequest>> FilterByStatus(string status)
    {
        return await _dbContext.LeaveRequests
            .Where(x => x.Status == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesCurrentlyOnLeave()
    {
        var today = DateTime.Today;

        return await _dbContext.Employees
            .Where(e => e.LeaveRequests.Any(l =>
                l.Status == "Approved" &&
                today >= l.StartDate &&
                today <= l.EndDate))
            .ToListAsync();
    }
}