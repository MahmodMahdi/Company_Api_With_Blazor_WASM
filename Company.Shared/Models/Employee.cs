using System.ComponentModel.DataAnnotations;

namespace Company.Shared.Models
{
	public class Employee
	{
		public int Id { get; set; }
		[Required]
		public string? Name { get; set; }
		[Required]
		[Range(2000,100000)]
		public decimal Salary { get; set; }
		[Required]
		public string? Address { get; set; }
		[Required]
		[Range(22, 50)]
		public int Age { get; set; }
		[Range(1, int.MaxValue, ErrorMessage = "Please select a department")]
		[Required(ErrorMessage = "Please select a department")]
		public int? departmentId { get; set; }

		public string? departmentName { get; set; }

	}
}
