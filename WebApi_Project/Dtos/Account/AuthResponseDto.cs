using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace Company_Management_System.Dtos.Account
{
	public class AuthResponseDto
	{
		public string? Token { get; set; }
		public DateTime? Expiration { get; set; }
	}
}
