
using Company.Shared;
using System.Net.Http.Json;

namespace MyCompany.BlazorUI.Repository.DepartmentRepository
{
	public class DepartmentRepository : IRepository<Department>
	{
		private readonly HttpClient _httpClient;
		public DepartmentRepository(HttpClient client)
		{
			_httpClient = client;
		}

		public async Task<GeneralResponse<IEnumerable<Department>>> GetAllAsync()
		{
			var departments =await _httpClient.GetFromJsonAsync<GeneralResponse<IEnumerable<Department>>>("api/Departments/GetAll");
			return departments ?? new GeneralResponse<IEnumerable<Department>>();
		}

		public async Task<GeneralResponse<Department>> GetByIdAsync(int id)
		{
			var department = await _httpClient.GetFromJsonAsync<GeneralResponse<Department>>($"api/Departments/GetByID/{id}");
			return department ?? new GeneralResponse<Department>();
		}

		public async Task CreateAsync(Department department)
		{
			await _httpClient.PostAsJsonAsync<Department>("api/Departments/Create", department);
		}

		public async Task UpdateAsync(int id,Department department)
		{
		    await _httpClient.PutAsJsonAsync<Department>($"api/Departments/Update/{id}", department);
		}
		public async Task<GeneralResponse<bool>> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"api/Departments/Delete/{id}");
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
