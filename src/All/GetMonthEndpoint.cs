using Tenda.All.Models;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.All;

public class GetMonthEndpoint : Endpoint<GetMonthRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/api/month/{Month}/year/{Year}");
        Claims("UserId");
    }

    public override async Task HandleAsync(GetMonthRequest req, CancellationToken ct)
    {
        var response = await GetByMonthService.GetMonth(req.Year, req.Month, req.UserId, ct);
        await SendAsync(response, cancellation: ct);
    }
}