using Microsoft.AspNetCore.SignalR;

namespace Tenda.Users;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        var claimId = connection.GetHttpContext()?.User.Claims.FirstOrDefault(x => x.Type == "UserId");
        return claimId?.Value.ToString();
    }
}