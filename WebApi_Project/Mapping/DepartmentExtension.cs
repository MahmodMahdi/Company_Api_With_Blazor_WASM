using Company_Management_System.Dtos.Deparment;
using WebApi_Demo.Models;

namespace Company_Management_System.Mapping
{
	public static class DepartmentExtension
	{
		public static DepartmentDto ToDto (this Department model)
		{
			return new DepartmentDto
			{
				Id = model.Id,
				Name = model.Name,
				DepartmentManager = model.DepartmentManager
			};
		}
		public static void ToModel (this DepartmentFormDto dto,Department department)
		{
			department.Name = dto.Name;
			department.DepartmentManager = dto.DepartmentManager;
		}
	}
}
