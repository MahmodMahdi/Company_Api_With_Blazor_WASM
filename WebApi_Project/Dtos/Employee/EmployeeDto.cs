using System.ComponentModel.DataAnnotations;

namespace Company_Management_System.Dtos.Employee;
public class EmployeeDto
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public decimal Salary { get; set; }
	public string? Address { get; set; }
	public int Age { get; set; }
	public int? departmentId { get; set; }
	public string? departmentName { get; set; }
}