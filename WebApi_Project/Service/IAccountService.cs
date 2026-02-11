using Company_Management_System.Dtos.Account;
using WebApi_Demo.Authentication;
using WebApi_Demo.Models;

namespace Company_Management_System.Service
{
	public interface IAccountService
	{
		Task<GeneralResponse<AuthResponseDto>> RegisterAsync(RegisterUserDto UserDto);
		Task<GeneralResponse<AuthResponseDto>> LoginAsync(LoginUserDto UserDto);
	}
}
