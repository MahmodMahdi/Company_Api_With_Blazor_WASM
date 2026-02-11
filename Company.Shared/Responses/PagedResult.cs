using Microsoft.EntityFrameworkCore;

namespace Company.Shared.Responses
{
	public class PagedResult<T>
	{
		public IEnumerable<T> Items { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }
		public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
		public bool HasPrevious => PageNumber > 1;
		public bool HasNext => PageNumber < TotalPages; 

		// ✅ أضف Default Constructor للـ JSON Deserialization
		public PagedResult()
		{
			Items = new List<T>();
		}
		public PagedResult(IEnumerable<T> items,int count, int pageNumber, int pageSize)
		{
			Items = items;
			TotalCount = count;
			PageNumber = pageNumber;
			PageSize = pageSize;
		}
		public static async Task<PagedResult<T>> GetPaginated (IQueryable<T> source, int pageNumber, int pageSize)
		{
			var count =await source.CountAsync();
			var items = await source
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			return new PagedResult<T>(items, count, pageNumber, pageSize);
		}
	}
}
