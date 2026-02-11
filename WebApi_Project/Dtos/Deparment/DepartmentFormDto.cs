using System.ComponentModel.DataAnnotations;

namespace Company_Management_System.Dtos.Deparment
{
	public class DepartmentFormDto
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string DepartmentManager { get; set; }
	}
}
