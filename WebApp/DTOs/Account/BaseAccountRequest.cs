using System.ComponentModel.DataAnnotations;

namespace WebApp.DTOs.Account;

public abstract class BaseAccountRequest
{
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public required string Password { get; set; }

    [Required] [StringLength(50, MinimumLength = 3)] public required string Username { get; set; }
}