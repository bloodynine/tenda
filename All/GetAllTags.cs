using FastEndpoints;
using MongoDB.Entities;
using Tenda.All.Models;
using Tenda.Shared.Models;

namespace Tenda.All;

public class GetAllTags : Endpoint<GetTagsRequest, List<string>>
{
    public override void Configure()
    {
        Get("api/tags");
        Claims("UserId");
    }

    public override async Task HandleAsync(GetTagsRequest req, CancellationToken ct)
    {
        var transactions = await DB.Find<FinancialTransaction>()
            .Match(x => x.UserId == req.UserId)
            .Project(x => x.Include("Tags")).ExecuteAsync(ct);
        var tags = transactions.SelectMany(x => x.Tags).Distinct();
        await SendAsync(tags.ToList(), cancellation: ct);
    }
}