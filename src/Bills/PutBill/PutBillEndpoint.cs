using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Bills.PutBill;

public class PutBillEndpoint : Endpoint<PutBillRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Put("api/bills/{Id}");
        Claims("UserId", "SeedId");
        PostProcessors(new TotalPostProcessor<PutBillRequest, Month>());
    }

    public override async Task HandleAsync(PutBillRequest req, CancellationToken ct)
    {
        var transaction = req.ToTransaction();
        var updatedBill = await DB.UpdateAndGet<FinancialTransaction>()
            .Match(x => x.ID == req.Id)
            .Match(x => x.UserId == req.UserId)
            .ModifyExcept(x => new { x.Type, x.AssociatedRepeatId }, transaction)
            .ExecuteAsync(ct);

        if (updatedBill is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(await GetByMonthService.GetMonth(req.Date, req.UserId, ct), cancellation: ct);
    }
}