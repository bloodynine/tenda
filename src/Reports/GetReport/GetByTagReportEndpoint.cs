using MongoDB.Entities;
using Tenda.Shared.Models;

namespace Tenda.Reports.GetReport;

public class GetByTagReportEndpoint : Endpoint<GetReportRequest, GetByTagResponse>
{
    public override void Configure()
    {
        Get("/api/reports/GetByTagReport");
        Claims("UserId");
    }

    public override async Task HandleAsync(GetReportRequest req, CancellationToken ct)
    {
        var transactionsQuery = DB.Find<FinancialTransaction>()
            .Match(x => x.UserId == req.UserId);

        if (req.DateRange.StartDate is not null)
        {
            transactionsQuery
                .Match(x => x.Date >= req.DateRange.StartDate && x.Date <= req.DateRange.EndDate);
        }

        var transactions = await transactionsQuery.ExecuteAsync(ct);

        var allTags = transactions.SelectMany(x => x.Tags).Distinct(StringComparer.CurrentCultureIgnoreCase);
        var ret = new GetByTagResponse();
        foreach (var tag in allTags)
        {
            var count = transactions.Count(x => x.Tags.Contains(tag));
            var total = transactions.Where(x => x.Tags.Contains(tag)).Sum(x => x.Amount);
            ret.ByCount.Add(new LabelDecimal(){ Name = tag, Value = count});
            ret.ByDollar.Add(new LabelDecimal() { Name = tag, Value = total});
        }

        await SendAsync(ret, cancellation: ct);
    }
}