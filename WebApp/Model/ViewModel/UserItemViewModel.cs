using System.Diagnostics.CodeAnalysis;
using WebApp.DTOs.Users;
using WebApp.Model.Enum;

namespace WebApp.Model.ViewModel;

public class UserItemViewModel
{
    [SetsRequiredMembers]
    public UserItemViewModel(List<UserResponse> userViewModels, string title)
    {
        UserViewModels = userViewModels;
        Title = title;
    }

    [SetsRequiredMembers]
    public UserItemViewModel(List<UserResponse> userViewModels, string buttonName, string title,
        FriendStatus status)
    {
        UserViewModels = userViewModels;
        FriendStatus = status;
        ButtonName = buttonName;
        Title = title;
    }

    public required  List<UserResponse> UserViewModels { get; set; }
    public FriendStatus? FriendStatus { get; set; }
    public  string? ButtonName { get; set; }
    public required string Title { get; set; }
}