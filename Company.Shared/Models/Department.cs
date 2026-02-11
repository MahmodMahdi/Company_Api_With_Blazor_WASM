using Company.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Shared
{
	public class Department
	{
		public int Id { get; set; }
		[Required]
		public string? Name { get; set; }
		[Required]
		public string? DepartmentManager { get; set; }
		public virtual List<Employee>? Employees { get; set; }
	}
}
