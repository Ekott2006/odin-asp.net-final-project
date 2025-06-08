using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Helper;
using WebApp.Model;
using LoginRequest = WebApp.DTOs.Account.LoginRequest;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebApp.Areas.Identity.Pages.Account;

public class LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger, SeedDatabase seedDatabase)
    : PageModel
{
    [BindProperty] public LoginRequest LoginRequest { get; set; }

    public string? ReturnUrl { get; set; }

    [TempData] public string ErrorMessage { get; set; }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        if (!ModelState.IsValid) return Page();
        return await LoginAsync(LoginRequest, returnUrl);
    }

    public async Task<IActionResult> LoginAsync(LoginRequest request, string returnUrl)
    {
        SignInResult result = await signInManager.PasswordSignInAsync(request.Username, request.Password,
            request.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            logger.LogInformation("User logged in.");
            return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();
    }

    public async Task<IActionResult> OnPostDemoAsync(int id)
    {
        string returnUrl = Url.Content("~/");
        LoginRequest request = new()
            { Username = string.Empty, Password = SeedDatabase.Password, RememberMe = true };
        switch (id)
        {
            case 1:
                request.Username = seedDatabase.UserNames[0];
                return await LoginAsync(request, returnUrl);
            case 2:
            default:
                request.Username = seedDatabase.UserNames[1];
                return await LoginAsync(request, returnUrl);
        }
    }
}