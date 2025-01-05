using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Application.Contracts.Account;

public class RegisterDto
{
    [Required(ErrorMessage = "Username is required.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "You have to confirm the password.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

}
