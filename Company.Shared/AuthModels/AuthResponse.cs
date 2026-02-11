using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Shared.AuthModels
{
	public class AuthResponse
	{
		public string? Token { get; set; }
		public DateTime? Expiration { get; set; }
	}
}
