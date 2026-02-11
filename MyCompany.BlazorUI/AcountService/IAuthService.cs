using Company.Shared.AuthModels;

namespace MyCompany.BlazorUI.AcountService
{
	public interface IAuthService
	{
		Task<GeneralResponse<AuthResponse>> Register(RegisterUser dto);
		Task<GeneralResponse<AuthResponse>> Login(LoginUser dto);
		Task Logout();
	}
}
