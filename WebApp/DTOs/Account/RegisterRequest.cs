using System.ComponentModel.DataAnnotations;

namespace WebApp.DTOs.Account;

public class RegisterRequest: BaseAccountRequest
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public required string Email { get; set; }

    
}