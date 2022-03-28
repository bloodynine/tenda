using System.Globalization;
using System.Reflection;
using MongoDB.Entities;
using Tenda.Shared.Extensions;
using Tenda.Shared.Models;

namespace Tenda.Reports.GetByMonthReport;

public class GetByMonthReportEndpoint : Endpoint<GetByMonthRequest, GetByMonthResponse>
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

        var expenseAvg = transactions.Where(x => TransactionTypeExtensions.ExpenseTransactions.Contains(x.Type))
            .Select(x => x.Amount).Sum() / 12;

        var incomeAvg = transactions.Where(x => x.Type == TransactionType.Income)
            .Select(x => x.Amount).Sum() / 12;
        GetByMonthResponse response = new()
        {
            IncomeExpensesByMonth = GetGroupedItems(req, transactionDictionary),
            AverageExpenses = expenseAvg,
            AverageIncome = incomeAvg
        };

        await SendAsync(response, cancellation: ct);
    }


    private IEnumerable<GroupedItem> GetGroupedItems(GetByMonthRequest req,
        Dictionary<int, IEnumerable<FinancialTransaction>>? transactions)
    {
        if (transactions is null) yield return null!;

        for (var i = 1; i < 13; i++)
        {
            var income = transactions!.GetValueOrDefault(i)?.Where(x => x.Type == TransactionType.Income).Select(y => Math.Abs(y.Amount));
            var expenses = transactions!.GetValueOrDefault(i)?.Where(x => x.Type != TransactionType.Income).Select(y => Math.Abs(y.Amount));
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