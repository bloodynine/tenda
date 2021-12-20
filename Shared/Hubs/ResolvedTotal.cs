using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Tenda.Shared.Hubs;

[Authorize]
public class ResolvedTotal : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("Foo", "User", "Message");
    }

    public override async Task OnConnectedAsync()
    {
        var cow = this.Context.User?.Claims.ToList();
        var moo = "cow";
    }
}