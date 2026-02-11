using System.ComponentModel.DataAnnotations;

namespace Company_Management_System.Dtos.Employee
{
	public class EmployeeFormDto
	{
		[Required]
		[MinLength(3)]
		public string? Name { get; set; }
		[Range(2000, 100000)]
		[Required]
		public decimal Salary { get; set; }
		[Required]
		public string? Address { get; set; }
		[Required]
		[Range(22,50)]
		public int Age { get; set; }
		[Required]
		public int? departmentId { get; set; }
	}
}
