using MongoDB.Entities;
using Tenda.Shared.Models;

namespace Tenda.Shared.Services;

public class TotalService: ITotalService
{
    public async Task<Seed> CalculateTotal(string userId, string seedId, CancellationToken ct)
    {
        var total = await DB.Find<FinancialTransaction, decimal>()
            .Match(x => x.IsResolved && x.UserId == userId)
            .Project(x => x.Amount )
            .ExecuteAsync(ct);

        return await DB.UpdateAndGet<Seed>()
            .MatchID(seedId)
            .Modify(x => x.Amount, total.Sum())
            .ExecuteAsync(ct);
    }
}

public interface ITotalService
{
    Task<Seed> CalculateTotal(string userId, string seedId, CancellationToken ct);
}