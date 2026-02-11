using Company_Management_System.Dtos.Employee;
using WebApi_Demo.Models;

namespace Company_Management_System.Mapping
{
	public static class EmployeeExtension
	{
		public static EmployeeDto ToDto(this Employee employee)
		{
			return new EmployeeDto
			{
				Id = employee.Id,
				Name = employee.Name,
				Address = employee.Address,
				Age = employee.Age,
				Salary = employee.Salary,
				departmentId = employee.departmentId,
				departmentName = employee.Department?.Name ?? "No Department"

			};
		}
		public static void ToModel(this EmployeeFormDto createEmployeeDto,Employee employee)
		{
			employee.Name = createEmployeeDto.Name;
			employee.Address = createEmployeeDto.Address;
			employee.Age = createEmployeeDto.Age;
			employee.Salary = createEmployeeDto.Salary;
			employee.departmentId = createEmployeeDto.departmentId;
		}
	}
}
