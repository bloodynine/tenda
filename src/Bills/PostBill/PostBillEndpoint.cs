using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Bills.PostBill;

public class PostBillEndpoint : Endpoint<PostBillRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/api/bills");
        Claims("UserId", "SeedId");
        PostProcessors(new TotalPostProcessor<PostBillRequest, Month>());
    }

    public override async Task HandleAsync(PostBillRequest req, CancellationToken ct)
    {
        var transaction = req.ToTransaction();

        if (req.RepeatSettings is not null)
        {
            RepeatContracts repeatSettings = new(req.RepeatSettings, req, TransactionType.Bill);
            await repeatSettings.SaveAsync(cancellation: ct);
            var bills = repeatSettings.CalculateTargets();
            await bills.SaveAsync(cancellation: ct);
        }
        else
        {
            await transaction.SaveAsync(cancellation: ct);
        }

        await SendAsync(await GetByMonthService.GetMonth(req.Date, req.UserId, ct), cancellation: ct);
    }
}