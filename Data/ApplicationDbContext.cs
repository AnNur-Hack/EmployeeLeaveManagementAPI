using EmployeeLeaveManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<LeaveApproval> LeaveApprovals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<LeaveRequest>()
            .HasOne(x => x.Employee)
            .WithMany(x => x.LeaveRequests)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict); 

        
        modelBuilder.Entity<LeaveApproval>()
            .HasOne(x => x.LeaveRequest)
            .WithMany(x => x.LeaveApprovals)
            .HasForeignKey(x => x.LeaveRequestId)
            .OnDelete(DeleteBehavior.Cascade); 

        
        modelBuilder.Entity<LeaveApproval>()
            .HasOne(x => x.Approver)
            .WithMany(x => x.LeaveApprovals)
            .HasForeignKey(x => x.ApproverId)
            .OnDelete(DeleteBehavior.Restrict); 

        base.OnModelCreating(modelBuilder);
    }
}