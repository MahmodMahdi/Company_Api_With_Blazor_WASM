using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Company.Shared.AuthModels
{
	public class RegisterUser
	{
		[Required, MinLength(3), MaxLength(60)]
		public string? UserName { get; set; }
		[Required]
		[EmailAddress(ErrorMessage = "الإيميل غير صحيح")]
		public string? Email { get; set; }
		[Required]
		[StringLength(100, MinimumLength = 8, ErrorMessage = "الكلمة يجب أن تكون 8 أحرف على الأقل")]
		public string? Password { get; set; }
		[Required]
		[Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
		public string? ConfirmPassword { get; set; }
	}
}
