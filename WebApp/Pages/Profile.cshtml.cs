using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.DTOs.Posts;
using WebApp.DTOs.Users;
using WebApp.Helper;
using WebApp.Model.ViewModel;
using WebApp.Repository;

namespace WebApp.Pages;

public class ProfileModel(UserRepo userRepo, IWebHostEnvironment environment) : PageModel
{
    public UserAndCommentResponse GetUser { get; set; }
    public CreatePostViewModel CreatePostViewModel { get; set; }

    public UploadImageViewModel ImageViewModel { get; set; }
    [BindProperty] public UploadImageViewModelRequest UploadImageRequest { get; set; }
    [TempData] public string? ErrorMessage { get; set; }

    public Claim? GetClaim() => User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);


    // TODO: Handle Images(Profile picture)
    public async Task<IActionResult> OnGet()
    {
        Claim? claim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (claim is null) return Forbid();
        UserAndCommentResponse? user = await userRepo.GetToViewModel(claim.Value);
        if (user is null) return Forbid();

        GetUser = user;
        CreatePostViewModel = new CreatePostViewModel("./Profile");
        ImageViewModel = new UploadImageViewModel("./Profile", false, "./Profile");
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        
        if (!ModelState.IsValid)
        {
            ErrorMessage = "Unable to Upload Image Retry. See Modal for errors!!!!";
            return RedirectToPage();
        }

        string? filename = await FileUpload.UploadHelper(UploadImageRequest.UploadedFile, environment);
        if (filename is null)
        {
            ErrorMessage = "Unable to Upload Image";
            return RedirectToPage();
        }
        await userRepo.UpdateProfileImage(GetClaim()!.Value, filename);
        return RedirectToPagePermanent(UploadImageRequest.ReturnUrl);
    }
}