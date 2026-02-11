using Company_Management_System.Dtos.Deparment;

namespace WebApi_Demo.interfaces
{
	public interface IDepartmentRepository
	{
		Task<GeneralResponse<IEnumerable<DepartmentDto>>> GetAllAsync();
		Task<GeneralResponse<DepartmentWithEmployeesDto>> GetDepartmentWithEmployeesByIdAsync(int id);
		Task<GeneralResponse<DepartmentDto>> GetByIdAsync(int id);
		Task<GeneralResponse<int>> CreateAsync(DepartmentFormDto employee);
		Task<GeneralResponse<int>> UpdateAsync(int id, DepartmentFormDto employee);
		Task<GeneralResponse<bool>> DeleteAsync(int id);
	}
}
