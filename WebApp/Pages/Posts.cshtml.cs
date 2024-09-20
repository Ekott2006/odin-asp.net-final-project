using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.DTOs.Comments;
using WebApp.DTOs.Posts;
using WebApp.Repository;

namespace WebApp.Pages;

public class PostsModel(ILogger<PostsModel> logger, PostRepo postRepo, CommentRepo commentRepo) : PageModel
{
    public PostAndCommentResponse GetPostResponse { get; set; }
    public string UserId { get; set; }
    [BindProperty] public string? Comment { get; set; }
    [BindProperty(SupportsGet = true)] public string NewComment { get; set; }

    [BindProperty] public Guid CommentId { get; set; }
    [TempData] public string? ErrorMessage { get; set; }

    public Claim? GetClaim() => User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

    public async Task<IActionResult> OnGet(Guid id)
    {
        PostAndCommentResponse? post = await postRepo.GetById(id, UserId);
        if (post is null) return NotFound();
        
        UserId = GetClaim()!.Value;
        GetPostResponse = post;
        
        return Page();
    }
    public async Task<IActionResult> OnGetCreate(Guid id)
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "Unable to Create Comment!!!";
            return RedirectToPage();
        }
        
        await commentRepo.Create(new CreateCommentRequest
        {
            Content = NewComment,
            UserId = GetClaim()!.Value,
            PostId = id
        });
        return RedirectToPage();
    }


    public async Task<IActionResult> OnPostDelete(Guid id)
    {
        await commentRepo.Delete(CommentId);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEdit(Guid id)
    {
        if (Comment is null)
        {
            ErrorMessage = "Unable to Update Comment!!!";
            return RedirectToPage();
        }

        await commentRepo.Edit(new ModifyCommentRequest
        {
            Content = Comment,
            UserId = GetClaim()!.Value,
            CommentId = CommentId
        });
        return RedirectToPage();
    }

}