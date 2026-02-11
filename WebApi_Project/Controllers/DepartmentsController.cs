using Company_Management_System.Dtos.Deparment;
using Company_Management_System.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_Demo.interfaces;

namespace WebApi_Demo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DepartmentsController : ControllerBase
	{
		private readonly IDepartmentRepository _departmentRepository;
		public DepartmentsController(IDepartmentRepository departmentRepository)
		{
			_departmentRepository = departmentRepository;
		}
		[Authorize]
		[HttpGet("GetAll")]
		public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllAsync()
		{
			var departments = await _departmentRepository.GetAllAsync();
			return Ok(departments);
		}
		[Authorize]
		[HttpGet("GetById/{id}", Name = "DepartmentDetailsRoute")]
		public async Task<ActionResult<DepartmentDto>> GetDepartmentById([FromRoute] int id)
		{
			var department = await _departmentRepository.GetByIdAsync(id);
			if (department == null)
				return NotFound();
			return Ok(department);
		}
		[Authorize]
		[HttpGet("GetDepartmentWithEmployee/{id}")]
		public async Task<ActionResult<DepartmentWithEmployeesDto>> GetDepartmentWithEmployeeByIdAsync([FromRoute] int id)
		{
			var department = await _departmentRepository.GetDepartmentWithEmployeesByIdAsync(id);
			if(department == null)
				return NotFound("Department is not found");
			else
			return Ok(department);
		}
		[Authorize(Roles ="Admin")]
		[HttpPost("Create")]
		public async Task<ActionResult<DepartmentFormDto>> CreateAsync(DepartmentFormDto departmentDto)
		{
			if (ModelState.IsValid)
			{
				await _departmentRepository.CreateAsync(departmentDto);
				return Ok(new { message = "Department Created Successfully", data = departmentDto });
			}
			return BadRequest(ModelState);
		}
		[Authorize(Roles ="Admin")]
		[HttpPut("Update/{id}")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] DepartmentFormDto departmentDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _departmentRepository.UpdateAsync(id, departmentDto);
				if (!result.Success)
					return BadRequest(result.Message);
				return NoContent();
			
		}
		[Authorize(Roles ="Admin")]
		[HttpDelete("Delete/{id}")]
		public async Task<IActionResult> DeleteAsync([FromRoute] int id)
		{

				var result = await _departmentRepository.DeleteAsync(id);
				if(!result.Success)
					return BadRequest(result.Message);
				return Ok(result);
		}
	}
}
