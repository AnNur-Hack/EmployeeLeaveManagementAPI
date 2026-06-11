using EmployeeLeaveManagementAPI.Data;
using EmployeeLeaveManagementAPI.DTOs;
using EmployeeLeaveManagementAPI.Models;
using Microsoft.EntityFrameworkCore;







namespace EmployeeLeaveManagementAPI.Repositories;

public class LeaveRepository : ILeaveRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LeaveRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<LeaveResponseDto>> GetAllLeaves()
    {
        var leaves = await _dbContext.LeaveRequests
            .Include(x => x.Employee)
            .Include(x => x.LeaveApprovals)
            .ThenInclude(x => x.Approver)
            .ToListAsync();

        if (leaves.Count == 0)
        {
            throw new Exception("No leave requests found");
        }

        var result = new List<LeaveResponseDto>();
        foreach (var leave in leaves)
        {
            result.Add(MapToLeaveResponse(leave));
        }
        return result;
    }

    public async Task<LeaveResponseDto> GetLeaveById(int id)
    {
        var leave = await _dbContext.LeaveRequests
            .Include(x => x.Employee)
            .Include(x => x.LeaveApprovals)
            .ThenInclude(x => x.Approver)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (leave == null)
        {
            throw new Exception($"Leave request with Id {id} not found");
        }

        return MapToLeaveResponse(leave);
    }

    public async Task<LeaveResponseDto> CreateLeave(SubmitLeaveDto dto)
    {
        var employee = await _dbContext.Employees.FindAsync(dto.EmployeeId);
        if (employee == null)
        {
            throw new Exception($"Employee with Id {dto.EmployeeId} not found");
        }

        var overlap = await _dbContext.LeaveRequests.AnyAsync(x =>
            x.EmployeeId == dto.EmployeeId &&
            x.Status != "Rejected" &&
            dto.StartDate <= x.EndDate &&
            dto.EndDate >= x.StartDate);

        if (overlap)
        {
            throw new Exception("Employee already has a leave request during this period");
        }

        var leave = new LeaveRequest
        {
            EmployeeId = dto.EmployeeId,
            LeaveType = dto.LeaveType.ToUpper(),
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Reason = dto.Reason,
            DateCreated = DateTime.Now
        };

        await _dbContext.LeaveRequests.AddAsync(leave);
        await _dbContext.SaveChangesAsync();

        return await GetLeaveById(leave.Id);
    }

    public async Task<LeaveResponseDto> UpdateLeave(int id, SubmitLeaveDto dto)
    {
        var leave = await _dbContext.LeaveRequests.FindAsync(id);
        
        if (leave == null)
        {
            throw new Exception($"Leave request with Id {id} not found");
        }

        if (leave.Status != "Pending")
        {
            throw new Exception("Only pending leave requests can be updated");
        }

        leave.LeaveType = dto.LeaveType.ToUpper();
        leave.StartDate = dto.StartDate;
        leave.EndDate = dto.EndDate;
        leave.Reason = dto.Reason;

        await _dbContext.SaveChangesAsync();

        return await GetLeaveById(id);
    }

    public async Task<bool> DeleteLeave(int id)
    {
        var leave = await _dbContext.LeaveRequests.FindAsync(id);
        
        if (leave == null)
        {
            throw new Exception($"Leave request with Id {id} not found");
        }

        _dbContext.LeaveRequests.Remove(leave);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }

    public async Task<LeaveResponseDto> ApproveLeave(int id, LeaveActionDto dto)
    {
        var leave = await _dbContext.LeaveRequests
            .Include(x => x.LeaveApprovals)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (leave == null)
        {
            throw new Exception($"Leave request with Id {id} not found");
        }

        if (leave.EmployeeId == dto.ApproverId)
        {
            throw new Exception("Employees cannot approve their own leave requests");
        }

        var alreadyActed = leave.LeaveApprovals.Any(x => x.ApproverId == dto.ApproverId);
        if (alreadyActed)
        {
            throw new Exception("You have already taken an action on this request");
        }

        if (leave.Status == "Approved")
        {
            throw new Exception("Leave request is already approved");
        }

        if (leave.Status == "Rejected")
        {
            throw new Exception("Leave request has been rejected");
        }

        var approval = new LeaveApproval
        {
            LeaveRequestId = id,
            ApproverId = dto.ApproverId,
            Action = "Approve",
            Reason = dto.Reason,
            DateActed = DateTime.Now
        };

        await _dbContext.LeaveApprovals.AddAsync(approval);
        
        leave.ApprovalCount++;

        if (leave.ApprovalCount == 1)
        {
            leave.Status = "Processing";
        }
        else if (leave.ApprovalCount == 2)
        {
            leave.Status = "Approved";
        }

        await _dbContext.SaveChangesAsync();

        return await GetLeaveById(id);
    }

    public async Task<LeaveResponseDto> RejectLeave(int id, LeaveActionDto dto)
    {
        var leave = await _dbContext.LeaveRequests
            .Include(x => x.LeaveApprovals)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (leave == null)
        {
            throw new Exception($"Leave request with Id {id} not found");
        }

        if (leave.EmployeeId == dto.ApproverId)
        {
            throw new Exception("Employees cannot reject their own leave requests");
        }

        var alreadyActed = leave.LeaveApprovals.Any(x => x.ApproverId == dto.ApproverId);
        if (alreadyActed)
        {
            throw new Exception("You have already taken an action on this request");
        }

        if (leave.Status == "Approved")
        {
            throw new Exception("Leave request is already approved");
        }

        if (leave.Status == "Rejected")
        {
            throw new Exception("Leave request has already been rejected");
        }

        var approval = new LeaveApproval
        {
            LeaveRequestId = id,
            ApproverId = dto.ApproverId,
            Action = "Reject",
            Reason = dto.Reason,
            DateActed = DateTime.Now
        };

        await _dbContext.LeaveApprovals.AddAsync(approval);
        
        leave.Status = "Rejected";

        await _dbContext.SaveChangesAsync();

        return await GetLeaveById(id);
    }

    public async Task<List<LeaveResponseDto>> FilterByStatus(string status)
    {
        var leaves = await _dbContext.LeaveRequests
            .Include(x => x.Employee)
            .Include(x => x.LeaveApprovals)
            .ThenInclude(x => x.Approver)
            .Where(x => x.Status == status)
            .ToListAsync();

        var result = new List<LeaveResponseDto>();
        foreach (var leave in leaves)
        {
            result.Add(MapToLeaveResponse(leave));
        }
        return result;
    }

    public async Task<List<LeaveStatisticsDto>> GetLeaveStatistics()
    {
        var departments = await _dbContext.Employees
            .Select(x => x.Department)
            .Distinct()
            .ToListAsync();

        var result = new List<LeaveStatisticsDto>();

        foreach (var dept in departments)
        {
            var leaves = await _dbContext.LeaveRequests
                .Include(x => x.Employee)
                .Where(x => x.Employee.Department == dept)
                .ToListAsync();

            result.Add(new LeaveStatisticsDto
            {
                Department = dept,
                TotalLeaveRequests = leaves.Count,
                ApprovedRequests = leaves.Count(x => x.Status == "Approved"),
                RejectedRequests = leaves.Count(x => x.Status == "Rejected"),
                PendingRequests = leaves.Count(x => x.Status == "Pending" || x.Status == "Processing")
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