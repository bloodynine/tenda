using MongoDB.Entities;
using Tenda.Shared.Models;

namespace Tenda.Shared.Services;

public class GetByMonthService : IGetByMonthService
{
    public async Task<Month> GetMonth(int year, int month, string userId, CancellationToken ct)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddTicks(-1);
        var transactions = await DB.Find<FinancialTransaction>()
            .Match(x => x.Date >= startDate)
            .Match(x => x.Date <= endDate)
            .ExecuteAsync(ct);
        var seed = await DB.Find<Seed>().Match(x => x.UserId == userId).ExecuteFirstAsync(ct);
        return new Month(startDate, endDate, seed!.Amount, new TransactionCollection(transactions));
    }

    public async Task<Month> GetMonth(DateTime date, string userId, CancellationToken ct)
    {
        return await GetMonth(date.Year, date.Month, userId, ct);
    }
}

public interface IGetByMonthService
{
    Task<Month> GetMonth(int year, int month, string userId, CancellationToken ct);
    Task<Month> GetMonth(DateTime date, string userId, CancellationToken ct);
}