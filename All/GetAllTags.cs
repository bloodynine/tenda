using Microsoft.Extensions.Caching.Memory;
using MongoDB.Entities;
using Tenda.All.Models;
using Tenda.Shared.Extensions;
using Tenda.Shared.Models;

namespace Tenda.All;

public class GetAllTags : Endpoint<GetTagsRequest, List<string>>
{
    public IMemoryCache Cache { get; set; } = null!;

    public override void Configure()
    {
        Get("api/tags");
        Claims("UserId");
    }

    public override async Task HandleAsync(GetTagsRequest req, CancellationToken ct)
    {
        var result = Cache.TryGetValue(req.UserId.ToTagKey(), out List<string> tags);
        if (result)
        {
            await SendAsync(tags.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList(), cancellation: ct);
        }
        else
        {
            var transactions = await DB.Find<FinancialTransaction>()
                    .Match(x => x.UserId == req.UserId)
                    .Project(x => x.Include("Tags")).ExecuteAsync(ct);
            var transactionTags = transactions.SelectMany(x => x.Tags).Distinct(StringComparer.CurrentCultureIgnoreCase);
            Cache.Set(req.UserId.ToTagKey(), transactionTags.ToList(), TimeSpan.FromMinutes(10));
            await SendAsync(transactionTags.ToList(), cancellation: ct);
        }
    }
}