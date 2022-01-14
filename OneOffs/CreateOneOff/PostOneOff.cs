using FastEndpoints;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Hubs;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.OneOffs.CreateOneOff;

public class PostOneOff : Endpoint<CreateOneOffRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Post("api/oneOffs");
        Claims("UserId");
        PostProcessors(new TotalPostProcessor<CreateOneOffRequest, Month>());
    }

    public override async Task HandleAsync(CreateOneOffRequest req, CancellationToken ct)
    {
        var transaction = req.ToTransaction();
        await transaction.SaveAsync(cancellation: ct);

        await SendAsync(await GetByMonthService.GetMonth(req.Date, req.UserId, ct), cancellation: ct);
    }
}