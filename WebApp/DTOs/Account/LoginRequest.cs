using System.ComponentModel.DataAnnotations;

namespace WebApp.DTOs.Account;

public class LoginRequest: BaseAccountRequest
{
    [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
}
