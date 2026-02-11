using Company_Management_System.Dtos.Deparment;
using Company_Management_System.Dtos.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_Demo.interfaces;

namespace WebApi_Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeesController : ControllerBase
{
	private readonly IEmployeeRepository employeeRepository;
	public EmployeesController(IEmployeeRepository employeeRepository)
	{
		this.employeeRepository = employeeRepository;
	}
	[HttpGet]
	public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllAsync(int pageNumber = 1,int pageSize = 10,string? search = null)
	{
		var employees = await employeeRepository.GetAllAsync(pageNumber,pageSize,search);
		return (Ok(employees));
	}
	[HttpGet("GetById/{id}", Name = "EmployeeDetailsRoute")]
	public async Task<ActionResult<EmployeeDto>> GetByIdAsync([FromRoute] int id)
	{
		var employee = await employeeRepository.GetByIdAsync(id);
		if (employee == null)
			return NotFound();
		return Ok(employee);
	}
	[Authorize(Roles ="Admin")]
	[HttpPost("Create")]
	public async Task<ActionResult<EmployeeFormDto>> CreateAsync(EmployeeFormDto employeeDto)
	{
		if (ModelState.IsValid)
		{
			var result = await employeeRepository.CreateAsync(employeeDto);
			if (!result.Success)
				return BadRequest(result.Message);
			return Ok(new { message = "Employee Created Successfully", data = employeeDto });
		}
		return BadRequest(ModelState);
	}
	[Authorize(Roles = "Admin")]
	[HttpPut("Update/{id}")]
	public async Task<IActionResult> UpdateAsync([FromRoute] int id , [FromBody] EmployeeFormDto employeeDto)
	{
		if (ModelState.IsValid)
		{
			var result = await employeeRepository.UpdateAsync(id, employeeDto);
			if (!result.Success)
				return BadRequest(result.Message);
			return NoContent();
		}
		return BadRequest(ModelState);
	}
	[Authorize(Roles = "Admin")]
	[HttpDelete("Delete/{id}")]
	public async Task<IActionResult> DeleteAsync([FromRoute] int id)
	{
		var result = await employeeRepository.DeleteAsync(id);
		if (!result.Success)
			return BadRequest(result.Message);
		return Ok(result);
	}
}
