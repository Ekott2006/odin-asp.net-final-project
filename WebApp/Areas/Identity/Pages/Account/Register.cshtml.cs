using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Model;
using RegisterRequest = WebApp.DTOs.Account.RegisterRequest;

namespace WebApp.Areas.Identity.Pages.Account;

public class RegisterModel(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ILogger<RegisterModel> logger)
    : PageModel
{
    [BindProperty] public RegisterRequest Input { get; set; }

    public string? ReturnUrl { get; set; }

    public void OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        if (!ModelState.IsValid) return Page();

        User user = new()
        {
            UserName = Input.Username, Email = Input.Email, ProfilePicture = "https://gravatar.com/avatar/" + ComputeSha256Hash(Input.Email.ToLower().Trim())
        };
        IdentityResult result = await userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
            logger.LogInformation("User created a new account with password.");
            await signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        foreach (IdentityError error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    private static string ComputeSha256Hash(string rawData)
    {
        // Create a SHA256
        // ComputeHash - returns byte array
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));

        // Convert byte array to a string
        StringBuilder builder = new();
        foreach (byte t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }
        return builder.ToString();
    }
}