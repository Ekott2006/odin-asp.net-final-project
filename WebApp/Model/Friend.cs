using WebApp.Model.Enum;

namespace WebApp.Model;

public class Friend
{
    public Guid Id { get; set; }
    public required string SenderId { get; set; }
    public User Sender { get; set; }
    public required string ReceiverId { get; set; }
    public User Receiver { get; set; }
    public FriendStatus Status { get; set; }
}