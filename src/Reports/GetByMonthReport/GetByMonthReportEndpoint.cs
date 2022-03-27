using System.Globalization;
using MongoDB.Entities;
using Tenda.Shared.Models;

namespace Tenda.Reports.GetByMonthReport;

public class GetByMonthReportEndpoint : Endpoint<GetByMonthRequest, List<GroupedItem>>
{
    public override void Configure()
    {
        Get("/api/reports/GetByMonthReport");
        Claims("UserId");
    }

    public override async Task HandleAsync(GetByMonthRequest req, CancellationToken ct)
    {
        var transactions = await DB.Find<FinancialTransaction>()
            .Match(x => x.Date >= req.GetStartDate())
            .Match(x => x.Date <= req.GetEndDate())
            .Match(x => x.UserId == req.UserId)
            .ExecuteAsync(ct);

        var transactionDictionary =
            transactions.GroupBy(x => x.Date.Month).ToDictionary(x => x.Key, x => x.Select(y => y));

        await SendAsync(GetGroupedItems(req, transactionDictionary).ToList(), cancellation: ct);
    }


    private IEnumerable<GroupedItem> GetGroupedItems(GetByMonthRequest req,
        Dictionary<int, IEnumerable<FinancialTransaction>>? transactions)
    {
        if (transactions is null) yield return null!;

        for (var i = 1; i < 13; i++)
        {
            var income = transactions!.GetValueOrDefault(i)?.Where(x => x.Type == TransactionType.Income).Select(y => y.Amount);
            var expenses = transactions!.GetValueOrDefault(i)?.Where(x => x.Type != TransactionType.Income).Select(y => y.Amount);
            var item = new GroupedItem
            {
                Name = new DateTime(req.Year, i, 7).ToString("MMMM", CultureInfo.InvariantCulture),
            };
            var series = new List<LabelDecimal>();
            if(income is not null) series.Add(new LabelDecimal("Income", income.Sum()));
            if(expenses is not null) series.Add(new LabelDecimal("Expenses", expenses.Sum()));
            item.Series = series;

            yield return item;
        }
    }
}