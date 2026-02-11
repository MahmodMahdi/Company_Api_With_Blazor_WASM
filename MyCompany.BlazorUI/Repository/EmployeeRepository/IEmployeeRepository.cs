using Company.Shared.Models;
using Company.Shared.Responses;

namespace MyCompany.BlazorUI.Repository.EmployeeRepository
{
	public interface IEmployeeRepository 
	{
		Task<GeneralResponse<PagedResult<Employee>>> GetPaged(int pageNumber, int pageSize, string? search);

	}
}
