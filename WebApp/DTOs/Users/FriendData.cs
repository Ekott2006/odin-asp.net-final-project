using WebApp.Model.Enum;

namespace WebApp.DTOs.Users;

public record FriendData(string Id, FriendStatus Status, bool IsSender);