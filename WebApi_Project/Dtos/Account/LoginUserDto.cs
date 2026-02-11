using System.ComponentModel.DataAnnotations;

namespace Company_Management_System.Dtos.Account;

public class LoginUserDto
{
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}
