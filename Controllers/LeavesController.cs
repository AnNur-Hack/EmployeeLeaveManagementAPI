using Microsoft.AspNetCore.Mvc;
using EmployeeLeaveManagementAPI.Repositories;
using EmployeeLeaveManagementAPI.DTOs;

namespace EmployeeLeaveManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class LeavesController : ControllerBase
{
    private readonly ILeaveRepository _leaveRepository;

    public LeavesController(ILeaveRepository leaveRepository)
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
    public async Task<IActionResult> CreateLeave(SubmitLeaveDto dto)
    {
        return Ok(await _leaveRepository.CreateLeave(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLeave(int id, SubmitLeaveDto dto)
    {
        return Ok(await _leaveRepository.UpdateLeave(id, dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeave(int id)
    {
        return Ok(await _leaveRepository.DeleteLeave(id));
    }

    [HttpPost("approve/{id}")]
    public async Task<IActionResult> ApproveLeave(int id, LeaveActionDto dto)
    {
        return Ok(await _leaveRepository.ApproveLeave(id, dto));
    }

    [HttpPost("reject/{id}")]
    public async Task<IActionResult> RejectLeave(int id, LeaveActionDto dto)
    {
        return Ok(await _leaveRepository.RejectLeave(id, dto));
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> FilterByStatus(string status)
    {
        return Ok(await _leaveRepository.FilterByStatus(status));
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetLeaveStatistics()
    {
        return Ok(await _leaveRepository.GetLeaveStatistics());
    }
}