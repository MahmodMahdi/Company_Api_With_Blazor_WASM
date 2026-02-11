using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Shared.AuthModels
{
	public class LoginUser
	{
		[Required]
		[EmailAddress(ErrorMessage = "الإيميل غير صحيح")]
		public string Email { get; set; }
		[Required]
		[Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
		public string Password { get; set; }
	}
}
