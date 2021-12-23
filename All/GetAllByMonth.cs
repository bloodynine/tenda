using FastEndpoints;
using Microsoft.AspNetCore.SignalR;
using Tenda.Shared.Hubs;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.All;

public class GetAllByMonth : Endpoint<GetAllRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("api/month/{Month}/year/{Year}");
        Claims("UserId");
    }

    public override async Task HandleAsync(GetAllRequest req, CancellationToken ct)
    {
        var response = await GetByMonthService.GetMonth(req.Year, req.Month, req.UserId, ct);
        await SendAsync(response, cancellation: ct);
    }
}