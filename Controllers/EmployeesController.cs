using Microsoft.AspNetCore.Mvc;
using EmployeeLeaveManagementAPI.Repositories;
using EmployeeLeaveManagementAPI.DTOs;

namespace EmployeeLeaveManagementAPI.Controllers;



[ApiController]
[Route("api/[controller]")]

public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        return Ok(await _employeeRepository.GetAllEmployees());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        return Ok(await _employeeRepository.GetEmployeeById(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
    {
        return Ok(await _employeeRepository.CreateEmployee(dto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDto dto)
    {
        return Ok(await _employeeRepository.UpdateEmployee(id, dto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        return Ok(await _employeeRepository.DeleteEmployee(id));
    }

    [HttpGet("leaves/{id}")]
    public async Task<IActionResult> GetEmployeeLeaveHistory(int id)
    {
        return Ok(await _employeeRepository.GetEmployeeLeaveHistory(id));
    }

    [HttpGet("on-leave")]
    public async Task<IActionResult> GetEmployeesCurrentlyOnLeave()
    {
        return Ok(await _employeeRepository.GetEmployeesCurrentlyOnLeave());
    }
}