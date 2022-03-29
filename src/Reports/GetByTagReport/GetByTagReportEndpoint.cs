using MongoDB.Entities;
using Tenda.Shared.Models;

namespace Tenda.Reports.GetByTagReport;

public class GetByTagReportEndpoint : Endpoint<GetReportRequest, GetByTagResponse>
{
    public override void Configure()
    {
        // This *should* be a GET endpoint, but there are too many bugs in FE query param deserialization to get it to work
        Post("/api/reports/GetByTagReport");
        Claims("UserId");
    }

    public override async Task HandleAsync(GetReportRequest req, CancellationToken ct)
    {
        var transactionsQuery = DB.Find<FinancialTransaction>()
            .Match(x => x.UserId == req.UserId);

        if (req.StartDate is not null && req.StartDate != new DateOnly())
        {
            transactionsQuery
                .Match(x => x.Date >= req.StartDate && x.Date <= req.EndDate);
        }

        if (req.Types?.Any() ?? false)
        {
            transactionsQuery.Match(x => req.Types.Contains(x.Type));
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