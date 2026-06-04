using EmployeeLeaveManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using EmployeeLeaveManagementAPI.Repositories;

namespace EmployeeLeaveManagementAPI.Controllers;


[ApiController]
[Route("api/[controller]")]

public class LeaveController : ControllerBase
{
    private readonly ILeaveRepository _leaveRepository;

    public LeaveController(ILeaveRepository leaveRepository)
    {
        _leaveRepository = leaveRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLeaves()
    {
        return Ok(await _leaveRepository.GetAllLeaves());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeaveById(int id)
    {
        return Ok(await _leaveRepository.GetLeaveById(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateLeave(CreateLeaveRequestDtos dto)
    {
        return Ok(await _leaveRepository.CreateLeave(dto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateLeave(LeaveRequest leave)
    {
        return Ok(await _leaveRepository.UpdateLeave(leave));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeave(int id)
    {
        return Ok(await _leaveRepository.DeleteLeave(id));
    }

    [HttpPut("approve/{id}")]
    public async Task<IActionResult> ApproveLeave(int id)
    {
        return Ok(await _leaveRepository.ApproveLeave(id));
    }

    [HttpPut("reject/{id}")]
    public async Task<IActionResult> RejectLeave(int id)
    {
        return Ok(await _leaveRepository.RejectLeave(id));
    }

    [HttpGet("status")]
    public async Task<IActionResult> FilterByStatus(string status)
    {
        return Ok(await _leaveRepository.FilterByStatus(status));
    }

    [HttpGet("current")]
    public async Task<IActionResult> EmployeesCurrentlyOnLeave()
    {
        return Ok(await _leaveRepository.GetEmployeesCurrentlyOnLeave());
    }
}