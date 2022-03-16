using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.OneOffs.PostOneOff;

public class PostOneOffEndpoint : Endpoint<PostOneOffRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Post("/api/oneOffs");
        Claims("UserId");
        PostProcessors(new TotalPostProcessor<PostOneOffRequest, Month>());
    }

    public override async Task HandleAsync(PostOneOffRequest req, CancellationToken ct)
    {
        var transaction = req.ToTransaction();
        await transaction.SaveAsync(cancellation: ct);

        await SendAsync(await GetByMonthService.GetMonth(req.Date, req.UserId, ct), cancellation: ct);
    }
}