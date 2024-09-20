using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.DTOs.Users;
using WebApp.Model;
using WebApp.Model.Enum;

namespace WebApp.Repository;

public class UserRepo(DataContext context)
{
    // REMOVE INCLUDES
    public async Task<User?> Get(string id) => await context.Users
        .Include(x => x.Posts).ThenInclude(x => x.Comments)
        .Include(x => x.Posts).ThenInclude(x => x.Likes)
        .Include(x => x.Posts).ThenInclude(x => x.PostDetail)
        .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<UserAndCommentResponse?> GetToViewModel(string id) =>await context.Users
        .Where(x => x.Id == id)
        .Include(x => x.Posts).ThenInclude(x => x.Comments)
        .Include(x => x.Posts).ThenInclude(x => x.Likes)
        .Include(x => x.Posts).ThenInclude(x => x.PostDetail)
        .Select(x =>  (UserAndCommentResponse) x).FirstOrDefaultAsync();
    public async Task<Dictionary<UserFriendStatus, List<UserResponse>>> GetAll(string id)
    {
        Dictionary<UserFriendStatus, List<UserResponse>> users = new()
        {
            { UserFriendStatus.Friend, [] },
            { UserFriendStatus.Sent, [] },
            { UserFriendStatus.NotSent, [] },
            { UserFriendStatus.ToAccept, [] },
        };

        List<UserResponse> allUsers = await context.Users.Where(x => x.Id != id).Select(x => UserResponse.FromUser(x)).ToListAsync();

        List<FriendData> friendDataList = await context.Friends.Where(x => x.ReceiverId == id || x.SenderId == id)
            .Select(x => new FriendData(x.SenderId == id ? x.ReceiverId : x.SenderId, x.Status, x.SenderId == id))
            .ToListAsync();
        foreach (UserResponse user in allUsers)
        {
            FriendData? friendData = friendDataList.FirstOrDefault(x => x.Id == user.Id);
            if (friendData is null)
            {
                users[UserFriendStatus.NotSent].Add(user);
                continue;
            }

            if (friendData.Status == FriendStatus.Accepted)
                users[UserFriendStatus.Friend].Add(user);
            else if (friendData.Status == FriendStatus.Sent)
                users[friendData.IsSender ? UserFriendStatus.Sent : UserFriendStatus.ToAccept].Add(user);

            friendDataList.Remove(friendData); // To reduce the iterations
        }

        return users;
    }

    public async Task UpdateProfileImage(string id, string profileImage) => await context.Users
        .Where(x => x.Id == id)
        .ExecuteUpdateAsync(x => x.SetProperty(y => y.ProfilePicture, profileImage));
}