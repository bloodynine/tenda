using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.OneOffs.PostOneOffs;

public class PostOneOffsEndpoint : Endpoint<PostOneOffsRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Post("/api/oneOffs/bulk");
        Claims("UserId", "SeedId");
        PostProcessors(new TotalPostProcessor<PostOneOffsRequest, Month>());
    }

    public override async Task HandleAsync(PostOneOffsRequest req, CancellationToken ct)
    {
        await req.ToTransactions().SaveAsync(cancellation: ct);
        await SendAsync(await GetByMonthService.GetMonth(req.OneOffs.First().Date, req.UserId, ct), cancellation: ct);
    }
}