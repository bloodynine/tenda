using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Shared.Hubs;

[Authorize]
public class ResolvedTotal : Hub
{
    private readonly ITotalService _totalService;
    private const string QueueName = "ResolvedTotal";

    public ResolvedTotal(ITotalService totalService)
    {
        _totalService = totalService;
    }

    public async Task UpdateSeedInformation(string userId, Seed? message)
    {
        if (message is not null) await Clients.User(userId).SendAsync(QueueName, message);
    }

    public override async Task OnConnectedAsync()
    {
        var claims = Context.User?.Claims.ToList();
        if (claims is not null)
        {
            var userId = claims.FirstOrDefault(x => x.Type == "UserId");
            var seedId = claims.FirstOrDefault(x => x.Type == "SeedId");
            var seed = await _totalService.CalculateTotal(userId!.Value, seedId!.Value, CancellationToken.None);
            await this.UpdateSeedInformation(userId!.Value, seed);
            // await Clients.User(userId.Value).SendAsync("Foo", seed, CancellationToken.None);
        }
    }
}