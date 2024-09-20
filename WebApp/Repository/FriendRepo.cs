using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Model;
using WebApp.Model.Enum;

namespace WebApp.Repository;

public class FriendRepo(DataContext context)
{
    // May be Optimized by FriendStatus.Sent to require only senderId and friendStatus.Denied to require only receiverId
    public IQueryable<Friend> GetByState(string id, FriendStatus status) => context.Friends
        .Include(x => x.Sender).Include(x => x.Receiver)
        .Where(x => (x.ReceiverId == id || x.SenderId == id) && x.Status == status);

    public async Task<Guid?> Modify(string senderId, string receiverId, FriendStatus newStatus = FriendStatus.Sent)
    {
        Friend newFriend = new()
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Status = newStatus,
        };
        Friend? friend = await context.Friends.AsSingleQuery().FirstOrDefaultAsync(x =>
            (x.SenderId == senderId && x.ReceiverId == receiverId) ||
            (x.SenderId == receiverId && x.ReceiverId == senderId));
        if (friend is not null) friend.Status = newStatus; 
        else await context.Friends.AddAsync(newFriend);
        await context.SaveChangesAsync();
        return newFriend.Id;
    }

    public async Task Modify(Guid id, FriendStatus newStatus) => await context.Friends.Where(x => x.Id == id)
        .ExecuteUpdateAsync(x => x.SetProperty(y => y.Status, newStatus));

    public async Task Delete(Guid id) => await context.Friends.Where(x => x.Id == id).ExecuteDeleteAsync();
}