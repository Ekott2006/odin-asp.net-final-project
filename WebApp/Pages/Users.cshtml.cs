using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.DTOs.Users;
using WebApp.Model.Enum;
using WebApp.Model.ViewModel;
using WebApp.Repository;

namespace WebApp.Pages;

public class UsersModel(UserRepo userRepo, FriendRepo friendRepo, ILogger<UsersModel> logger) : PageModel
{
    public List<UserItemViewModel> ViewModel { get; set; }
    [BindProperty] public string Id { get; set; }
    [BindProperty] public FriendStatus Status { get; set; }
    [TempData] public string? ErrorMessage { get; set; }

    public Claim? GetClaim() => User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

    public async Task<IActionResult> OnGet()
    {
        Dictionary<UserFriendStatus, List<UserResponse>> allUsers = await userRepo.GetAll(GetClaim()!.Value);
        ViewModel =
        [
            new UserItemViewModel(userViewModels: allUsers[UserFriendStatus.ToAccept], buttonName: "Accept",
                title: "Pending Request", status: FriendStatus.Accepted),
            new UserItemViewModel(userViewModels: allUsers[UserFriendStatus.NotSent],
                buttonName: "Send Request", title: "People you may Know", status: FriendStatus.Sent),
            new UserItemViewModel(userViewModels: allUsers[UserFriendStatus.Friend], title: "My Friends"),
            new UserItemViewModel(userViewModels: allUsers[UserFriendStatus.Sent], title: "Sent Requests"),
        ];

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "Unable to Modify Friendship status!!!";
            return RedirectToPage();
        }
        await friendRepo.Modify(GetClaim()!.Value, Id, Status);
        return RedirectToPage();
    }

}