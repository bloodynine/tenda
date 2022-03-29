using MongoDB.Entities;
using Tenda.Shared.Models;

namespace Tenda.Shared.Services;

public class GetByMonthService : IGetByMonthService
{
    public async Task<Month> GetMonth(int year, int month, string userId, CancellationToken ct)
    {
        var startDate = new DateOnly(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        var transactions = await DB.Find<FinancialTransaction>()
            .Match(x => x.Date >= startDate)
            .Match(x => x.Date <= endDate)
            .Match(x => x.UserId == userId)
            .ExecuteAsync(ct);

        var previousTransactions = await DB.Find<FinancialTransaction, decimal>()
            .Match(x => x.UserId == userId)
            .Match(x => x.Date < startDate)
            .Project(x => x.Amount)
            .ExecuteAsync(ct);

        decimal total = 0;
        if (previousTransactions.Count > 0) total = previousTransactions.Sum();
        return new Month(startDate, endDate, total, new TransactionCollection(transactions));
    }

    public async Task<Month> GetMonth(DateOnly date, string userId, CancellationToken ct)
    {
        return await GetMonth(date.Year, date.Month, userId, ct);
    }
}

public interface IGetByMonthService
{
    Task<Month> GetMonth(int year, int month, string userId, CancellationToken ct);
    Task<Month> GetMonth(DateOnly date, string userId, CancellationToken ct);
}