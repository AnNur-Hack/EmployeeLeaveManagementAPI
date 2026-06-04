using EmployeeLeaveManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using EmployeeLeaveManagementAPI.Repositories;
namespace EmployeeLeaveManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
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
    public async Task<IActionResult> CreateEmployee(CreateEmployeeDtos dto)
    {
        return Ok(await _employeeRepository.CreateEmployee(dto));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployee(Employee employee)
    {
        return Ok(await _employeeRepository.UpdateEmployee(employee));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        return Ok(await _employeeRepository.DeleteEmployee(id));
    }

    [HttpGet("{id}/leaves")]
    public async Task<IActionResult> GetEmployeeLeaveHistory(int id)
    {
        return Ok(await _employeeRepository.GetEmployeeLeaveHistory(id));
    }
}