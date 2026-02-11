using Company.Shared.Responses;

namespace MyCompany.BlazorUI.Repository
{
	public interface IRepository <T> where T : class
	{
		Task<GeneralResponse<IEnumerable<T>>> GetAllAsync ();
		Task<GeneralResponse<T>> GetByIdAsync (int id);
		Task CreateAsync (T entity);
		Task UpdateAsync (int id ,T entity);
		Task<GeneralResponse<bool>> DeleteAsync (int id);
	}
}
