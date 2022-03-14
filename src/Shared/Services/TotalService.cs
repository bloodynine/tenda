using MongoDB.Entities;
using Tenda.Shared.Models;

namespace Tenda.Shared.Services;

public class TotalService: ITotalService
{
    public async Task<Seed> CalculateTotal(string userId, string seedId, CancellationToken ct)
    {
        var totalDocs = await DB.Find<FinancialTransaction, decimal>()
            .Match(x => x.IsResolved && x.UserId == userId)
            .Project(x => x.Amount )
            .ExecuteAsync(ct);

        decimal total = 0;
        if (totalDocs.Count > 0) total = totalDocs.Sum();
        return await DB.UpdateAndGet<Seed>()
            .MatchID(seedId)
            .Modify(x => x.Amount, total)
            .ExecuteAsync(ct);
    }
}

public interface ITotalService
{
    Task<Seed> CalculateTotal(string userId, string seedId, CancellationToken ct);
}