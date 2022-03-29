using MongoDB.Entities;
using Tenda.Shared.Models;

namespace Tenda.Reports.GetReportSettings;

public class GetReportSettingsEndpoint : Endpoint<GetReportSettingsRequest, GetReportSettingsResponse>
{
    public override void Configure()
    {
        Get("/api/reports/settings");
        Claims("UserId");
    }

    public override async Task HandleAsync(GetReportSettingsRequest req, CancellationToken ct)
    {
        var dates = await DB.Find<FinancialTransaction, DateOnly>()
            .Match(x => x.UserId == req.UserId)
            .Project(x => x.Date )
            .ExecuteAsync(ct);
        var years = dates.Select(x => x.Year).Distinct().OrderByDescending(x => x);

        await SendAsync(new() { ValidYears = years.ToList() }, cancellation: ct);
    }
}