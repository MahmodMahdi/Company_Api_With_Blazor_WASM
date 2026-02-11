using Company_Management_System.Dtos.Account;
using Company_Management_System.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_Demo.Authentication;

namespace WebApi_Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    [HttpPost("register")] // api/Account/register
    public async Task<IActionResult> Register(RegisterUserDto userDto)
    {
        if (ModelState.IsValid)
        {
            var user = await _accountService.RegisterAsync(userDto);
            if (!user.Success)
                return BadRequest(user);
            return Ok(user);
		}
        return BadRequest(ModelState);
    }
    [HttpPost("Login")]  // api/Account/Login
    public async Task<IActionResult> Login(LoginUserDto userDto)
    {
		if (!ModelState.IsValid)
			return BadRequest(new GeneralResponse<AuthResponseDto>
			{
				Success = false,
				Message = "Invalid input",
				Data = null
			});
		var result =await _accountService.LoginAsync(userDto);
            if(!result.Success)
            return Unauthorized(new {message = result.Message});
            return Ok(result);
        
    }
}
