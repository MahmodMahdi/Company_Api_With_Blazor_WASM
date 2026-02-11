using Company.Shared.Models;
using Company.Shared.Responses;
using System.Net.Http.Json;
namespace MyCompany.BlazorUI.Repository.EmployeeRepository
{
	public class EmployeeRepository : IRepository<Employee>,IEmployeeRepository
	{
		private readonly HttpClient _httpClient;
		public EmployeeRepository(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}


		public async Task<GeneralResponse<IEnumerable<Employee>>> GetAllAsync()
		{
			var employees = await _httpClient.GetFromJsonAsync<GeneralResponse<IEnumerable<Employee>>>("api/Employees");
			return employees ?? new GeneralResponse<IEnumerable<Employee>>();
		}
		public async Task<GeneralResponse<PagedResult<Employee>>> GetPaged(int pageNumber ,int pageSize,string? search)
		{
			string url = $"api/Employees?pageNumber={pageNumber}&pageSize={pageSize}";

			if (!string.IsNullOrWhiteSpace(search))
			{
				url += $"&search={Uri.EscapeDataString(search)}"; // لو فيه مسافات أو رموز
			}
			var employees = await _httpClient.GetFromJsonAsync<GeneralResponse<PagedResult<Employee>>>(url);
			
			return employees ?? new GeneralResponse<PagedResult<Employee>>();
		}
		public async Task<GeneralResponse<Employee>> GetByIdAsync(int id)
		{
			var employee = await _httpClient.GetFromJsonAsync<GeneralResponse<Employee>>($"api/Employees/GetById/{id}");
			return employee ?? new GeneralResponse<Employee>();
		}

		public async Task CreateAsync(Employee employee)
		{
			var response = await _httpClient.PostAsJsonAsync<Employee>("api/Employees/Create", employee);
		}

		public async Task UpdateAsync(int id ,Employee employee)
		{
			var response = await _httpClient.PutAsJsonAsync<Employee>($"api/Employees/Update/{id}", employee);
		}
		public async Task<GeneralResponse<bool>> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"api/Employees/Delete/{id}");
			if (!response.IsSuccessStatusCode)
			{
				var error = await response.Content.ReadAsStringAsync();
				return new GeneralResponse<bool>(false, $"Error: {error}", false);
			}

			// Read the JSON response from API
			return await response.Content.ReadFromJsonAsync<GeneralResponse<bool>>()!;
		}

	}
}
