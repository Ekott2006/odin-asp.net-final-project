using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.DTOs.Posts;
using WebApp.Helper;
using WebApp.Model.ViewModel;
using WebApp.Repository;

namespace WebApp.Pages;

public class IndexModel(ILogger<IndexModel> logger, PostRepo postRepo, IWebHostEnvironment environment) : PageModel
{
    [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = 1;
    [BindProperty(SupportsGet = true)] public CreatePostViewModelRequest? CreatePostRequest { get; set; }
    [BindProperty] public UploadImageViewModelRequest UploadImageRequest { get; set; }
    public PaginatedPostsResponse PostsResponse { get; set; }
    public CreatePostViewModel CreatePostViewModel { get; set; }
    [TempData] public string? ErrorMessage { get; set; }

    public Claim? GetClaim() => User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

    public async Task<IActionResult> OnGet()
    {
        PostsResponse = await postRepo.GetByUserId(GetClaim()!.Value, PageNumber);
        CreatePostViewModel = new CreatePostViewModel("./Index");
        return Page();
    }

    public async Task<IActionResult> OnGetHandleLike(Guid id)
    {
        return await postRepo.HandleLike(id, GetClaim()!.Value) ? Content("") : NotFound();
    }

    // TODO: Handle Error Later
    public async Task<IActionResult> OnGetCreate(Guid id)
    {
        if (!ModelState.IsValid || CreatePostRequest is null)
        {
            ErrorMessage = "Unable to Create a new Post";
            return RedirectToPage();
        }

        await postRepo.Create(new CreatePostRequest(GetClaim()!.Value, CreatePostRequest.Content));
        return RedirectToPagePermanent(CreatePostRequest.ReturnUrl);
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
        await postRepo.Create(new CreatePostRequest
        {
            AuthorId = GetClaim()!.Value,
            ImageLink = filename,
            Caption = UploadImageRequest.Caption
        });
        return RedirectToPagePermanent(UploadImageRequest.ReturnUrl);
    }
}