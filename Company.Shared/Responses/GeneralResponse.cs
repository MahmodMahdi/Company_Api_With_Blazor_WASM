public class GeneralResponse<T>
{
	public bool Success { get; set; }
	public string? Message { get; set; }
	public T? Data { get; set; } // هنا تضع أي بيانات تريد إرجاعها

	// Constructor للنجاح مع بيانات
	public GeneralResponse(bool success, string? message, T? data = default)
	{
		Success = success;
		Message = message;
		Data = data;
	}

	// Constructor فارغ مهم جداً للـ JSON
	public GeneralResponse() { }
}