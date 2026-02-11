using Company_Management_System.Dtos.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_Demo.Authentication;
using WebApi_Demo.Models;

namespace Company_Management_System.Service
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _config;
		public AccountService(UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IConfiguration config)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_config = config;
		}
		private async Task<AuthResponseDto> GenerateToken(ApplicationUser user)
		{
			//Claims Token
			var claims = new List<Claim>
			{
				   new Claim(ClaimTypes.Name, user.Email!),
				   new Claim(ClaimTypes.NameIdentifier, user.Id),
				   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			// get role
			var roles = await _userManager.GetRolesAsync(user);
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			// key in SigningCredentials
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]!));
			// need key and algorithm
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			// Create Token
			var token = new JwtSecurityToken(
				issuer: _config["JWT:ValidIssuer"],
				audience: _config["JWT:ValidAudience"],
				claims: claims,
				expires: DateTime.UtcNow.AddDays(3),
				signingCredentials: credentials
			);

			return new AuthResponseDto
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Expiration = token.ValidTo
			};
		}

		// 2. تعديل الـ Register لترجع التوكن فوراً
		public async Task<GeneralResponse<AuthResponseDto>> RegisterAsync(RegisterUserDto userDto)
		{
			var exist = await _userManager.FindByEmailAsync(userDto.Email);
			if (exist != null)
				return new GeneralResponse<AuthResponseDto>(false, "Account already exists");

			var user = new ApplicationUser { UserName = userDto.Email, Email = userDto.Email };
			var result = await _userManager.CreateAsync(user, userDto.Password!);

			if (result.Succeeded)
			{
				if (!await _roleManager.RoleExistsAsync("User"))
					await _roleManager.CreateAsync(new IdentityRole { Name = "User" });

				await _userManager.AddToRoleAsync(user, "User");

				var authResponse = await GenerateToken(user);
				return new GeneralResponse<AuthResponseDto>(true, "Account Created & Logged In", authResponse);
			}

			var errors = string.Join(',', result.Errors.Select(s => s.Description));
			return new GeneralResponse<AuthResponseDto>(false, errors);
		}
		public async Task<GeneralResponse<AuthResponseDto>> LoginAsync(LoginUserDto userDto)
		{
			var user = await _userManager.FindByEmailAsync(userDto.Email!);

			if (user != null && await _userManager.CheckPasswordAsync(user, userDto.Password!))
			{
				// استدعاء ميثود منفصلة لتوليد التوكن
				var authResponse = await GenerateToken(user);

				return new GeneralResponse<AuthResponseDto>(true, "Login Successful", authResponse);
			}

			return new GeneralResponse<AuthResponseDto>(false, "Invalid Email or Password");
		}
	}
}

